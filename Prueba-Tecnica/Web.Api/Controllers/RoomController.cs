using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Core.Contracts.Services;
using Core.Models;
using Core.Models.Enums;
using Api.Models.Input;
using Api.Models.Output;

namespace Api.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;

        public RoomController(IMapper mapper, IRoomService roomService, IReservationService reservationService)
        {
            _mapper = mapper;
            _roomService = roomService;
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllAsync();

                var models = rooms.Select(e => _mapper.Map<Models.Output.RoomModel>(e));

                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpGet("{idRoom}")]
        public async Task<ActionResult<Models.Output.RoomModel>> GetRoom(int idRoom)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(idRoom);

                if (room == null)
                {
                    return NotFound();
                }

                return _mapper.Map<Models.Output.RoomModel>(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostRoom([FromBody] Models.Input.RoomModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var room = _mapper.Map<Room>(model);
                await _roomService.CreateAsync(room);

                return CreatedAtAction("PostRoom", new { id = room.Id }, _mapper.Map<Models.Output.RoomModel>(room));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpPut("{idRoom}")]
        public async Task<IActionResult> PutRoom(int idRoom, [FromBody] Models.Input.RoomModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idRoom != model.Id)
            {
                return BadRequest(new { Message = "El Id de la sala no coincide con el modelo." });
            }

            try
            {
                var oldRoom = await _roomService.GetByIdAsync(idRoom);

                if (oldRoom == null)
                    return NotFound(new { Message = $"La sala con Id: {idRoom} no existe." });

                var room = _mapper.Map<Room>(model);
                oldRoom.Name = room.Name;
                oldRoom.Capacity = room.Capacity;
                oldRoom.HasProjector = room.HasProjector;
                oldRoom.HasBlackboard = room.HasBlackboard;
                oldRoom.HasInternet = room.HasInternet;

                await _roomService.UpdateAsync(oldRoom);

                return Ok(new { Message = $"Se actualizó la sala con Id: {idRoom}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpDelete("{idRoom}")]
        public async Task<IActionResult> DeleteRoom(int idRoom)
        {
            try
            {
                var room = await _roomService.GetRoomWithReservationsAsync(idRoom);

                if (room == null)
                    return NotFound(new { Message = $"La sala con Id: {idRoom} no existe." });

                if (room.Reservations.Any(e => e.IdReservationState == (int)ReservationStatesEnum.Pendiente))
                    return Conflict(new { Message = $"La sala con Id: {idRoom} no puede eliminarse porque tiene reservas pendientes." });

                await _roomService.DeleteAsync(idRoom);

                return Ok(_mapper.Map<Models.Output.RoomModel>(room));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpPost("{idRoom}/reserve")]
        public async Task<IActionResult> Reserve(int idRoom, [FromBody] Models.Input.ReservationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var room = await _roomService.GetByIdAsync(idRoom);

                if (room == null)
                    return NotFound(new { Message = $"La sala con Id: {idRoom} no existe." });

                if (room.IsReserved)
                    return BadRequest(new { Message = $"La sala con Id: {idRoom} ya se encuentra reservada." });

                var reservation = _mapper.Map<Reservation>(model);
                reservation.IdRoom = idRoom;

                if (!ValidateReservation(room, reservation))
                    return BadRequest(new { Message = "La sala seleccionada no cumple con los requisitos de la reserva." });

                await _reservationService.CreateAsync(reservation);

                room.IsReserved = true;
                await _roomService.UpdateAsync(room);

                return CreatedAtRoute(new { IdRoom = idRoom }, _mapper.Map<Models.Output.ReservationModel>(reservation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpPost("{idRoom}/cancel-reservation")]
        public async Task<IActionResult> CancelReservation(int idRoom)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(idRoom);

                if (room == null)
                    return NotFound(new { Message = $"La sala con Id: {idRoom} no existe." });

                var reservation = await _reservationService.GetLastReservationByRoomAsync(idRoom);

                if (reservation == null)
                    return BadRequest(new { Message = $"No existen reservaciones pendientes para la sala con Id: {idRoom}." });

                reservation.IdReservationState = (int)ReservationStatesEnum.Cancelada;
                await _reservationService.UpdateAsync(reservation);

                room.IsReserved = false;
                await _roomService.UpdateAsync(room);

                return Ok(new { Message = $"Se canceló la reserva para la sala con Id: {idRoom}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpPost("{idRoom}/finish-reservation")]
        public async Task<IActionResult> FinishReservation(int idRoom)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(idRoom);

                if (room == null)
                    return NotFound(new { Message = $"La sala con Id: {idRoom} no existe." });

                var reservation = await _reservationService.GetLastReservationByRoomAsync(idRoom);

                if (reservation == null)
                    return BadRequest(new { Message = $"No existen reservaciones pendientes para la sala con Id: {idRoom}." });

                reservation.IdReservationState = (int)ReservationStatesEnum.Finalizada;
                await _reservationService.UpdateAsync(reservation);

                room.IsReserved = false;
                await _roomService.UpdateAsync(room);

                return Ok(new { Message = $"Se finalizó la reserva para la sala con Id: {idRoom}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> SuggestRooms([FromQuery] Models.Input.ReservationModel model)
        {
            try
            {
                var rooms = await _roomService.SearchByResourcesAsync(
                    model.NumberOfAssistants.Value,
                    model.UseProjector.Value,
                    model.UseBlackboard.Value,
                    model.UseInternet.Value);

                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GenerateReport([FromQuery] DateTime initDate, [FromQuery] DateTime finishDate)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsAsync(initDate, finishDate.AddDays(1));

                var result = reservations
                    .Where(e => e.IdReservationState != (int)ReservationStatesEnum.Cancelada)
                    .GroupBy(e => new { e.IdRoom, e.Room.Name })
                    .Select(g => new ReportModel
                    {
                        IdRoom = g.Key.IdRoom.Value,
                        RoomName = g.Key.Name,
                        TotalReservations = g.Count(),
                        TotalAssistants = g.Sum(e => e.NumberOfAssistants),
                        AverageAssistants = (int)g.Average(e => e.NumberOfAssistants),
                        ProjectorUtilization = g.Sum(e => e.UseProjector ? 1 : 0),
                        BlackboardUtilization = g.Sum(e => e.UseBlackboard ? 1 : 0),
                        InternetUtilization = g.Sum(e => e.UseInternet ? 1 : 0)
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ocurrió una excepción. {ex.Message}" });
            }
        }

        #region PRIVATE METHODS

        private bool ValidateReservation(Room room, Reservation reservation)
        {
            if (room.Capacity < reservation.NumberOfAssistants)
                return false;

            if (!room.HasProjector && reservation.UseProjector)
                return false;

            if (!room.HasBlackboard && reservation.UseBlackboard)
                return false;

            if (!room.HasInternet && reservation.UseInternet)
                return false;

            return true;
        }

        #endregion
    }
}
