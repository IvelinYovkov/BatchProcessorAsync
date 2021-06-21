namespace BuildJob.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [Route("[controller]")]
    [ApiController]
    public class BuildArtefactController : ControllerBase
    {
        readonly Random random = new Random();
        readonly String[] resultStrings = { "Unsuccess", "Success" };

        [HttpGet]
        public async Task<string> BuildArtefact(string name = "")
        {
            return await Task.Run(() => DoJob(name));           
        }

        private async Task<string> DoJob(string name)
        {
            int sleepTime = random.Next(2000, 30000);
            int resultIndex = random.Next(0, 2);
            await Task.Delay(sleepTime);
            return $"Name: {name}, Result: {resultStrings[resultIndex]}, Time: {sleepTime}";
        }
    }
}