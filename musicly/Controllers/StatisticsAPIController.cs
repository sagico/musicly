using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using musicly.Models;
using Microsoft.EntityFrameworkCore;
using musicly.Data;
using Newtonsoft.Json;

namespace musicly.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsAPIController : Controller
    {
        protected musiclyContext _context;
        public StatisticsAPIController(musiclyContext context)
        {
            this._context = context;
        }

        [HttpGet("types/prices")]
        public JsonResult TypesAveragePrices()
        {
            var data = _context.Instrument.GroupBy(p => p.InstrumentType.Name).
                Select(a => new { name = a.Key, value = a.Average(p => p.Price)}).
                OrderBy(a => a.value).
                ToArray();
            return Json(JsonConvert.SerializeObject(data));
        }

        [HttpGet("types/count")]
        public JsonResult TypesAmount()
        {
            var data = _context.Instrument.GroupBy(p => p.InstrumentType.Name).
                Select(a => new { name = a.Key, value = a.Count() }).                
                ToArray();
            return Json(JsonConvert.SerializeObject(data));
        }
    }
}