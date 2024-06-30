using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FileController));


        [HttpGet]
        public Result getList()
        {

            return new Result(0, "OK!");
        }

        [HttpGet]
        public Result getFile(int id)
        {
            return new Result(0, "OK!");
        }





    }
}
