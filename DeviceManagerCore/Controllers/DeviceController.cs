using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeviceManagerCore.Models;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using DeviceManagerCore.Hubs;
using DeviceManagerCore.Respository;
using System;

namespace DeviceManagerCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IHubContext<DeviceHub> _hubContext;
        private readonly IDeviceRespository _deviceRespository;

        public DeviceController(IHubContext<DeviceHub> hubContext, IDeviceRespository deviceRespository)
        {
            _hubContext = hubContext;
            _deviceRespository = deviceRespository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _deviceRespository.GetAllDevice());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _deviceRespository.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNew([Bind("deviceName, deviceQuantity, devicePrice")] Device dv)
        {
            try
            {
                bool result = await _deviceRespository.AddNew(dv);
                if (result)
                {
                    this.HubDetachDbChange();
                    return Ok();
                }
                return BadRequest("Cant not create new Device");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([Bind("deviceId, deviceName, deviceQuantity, devicePrice")] Device dv)
        {
            try
            {
                bool result = await _deviceRespository.Update(dv);
                if (result)
                {
                    this.HubDetachDbChange();
                    return CreatedAtAction(nameof(Update), dv, dv);
                }
                return BadRequest("Cant not update device");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([Bind("listId")] int[] listId)
        {
            try
            {
                bool success = await _deviceRespository.Delete(listId);
                if (success)
                {
                    this.HubDetachDbChange();
                    return Ok("Deleted");
                }
                else
                {
                    throw new Exception("Cant not delete device");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }





        }


        private void HubDetachDbChange()
        {
            _hubContext.Clients.All.SendAsync("DEVICE_CHANGED", "Server", "Db_changed");
        }

    }

}
