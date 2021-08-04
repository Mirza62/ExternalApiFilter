using DonateKartWebApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DonateKartWebApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        //use "api/campaign/GetCampaignResults" to hit the endpoint with the localhost url
        [HttpGet("GetCampaignResults")]
        public async Task<List<CampaignResult>> GetCampaignResults()
        {
            var model = await GetJsonDataFromExternalApi();
            var campaignList = model.Select(a => new CampaignResult() { title = a.title, totalAmount = a.totalAmount, backersCount = a.backersCount, endDate = a.endDate })
                .OrderByDescending(a => a.totalAmount).ToList();
            return campaignList;
        }

        //use "api/campaign/GetActiveCampaignsList" to hit the endpoint with the localhost url
        [HttpGet("GetActiveCampaignsList")]
        public async Task<ActionResult<List<Campaign>>> GetActiveCampaignsList()
        {
            DateTime todaysdate = DateTime.Now;
            DateTime PreviousOneMonth = DateTime.Now.AddMonths(-1);
            var model = await GetJsonDataFromExternalApi();
            var activeCampaignList = model.Where(a => a.created >= PreviousOneMonth && a.endDate >= todaysdate).ToList();
            return activeCampaignList;
        }

        //use "api/campaign/GetClosedCampaignsList" to hit the endpoint with the localhost url
        [HttpGet("GetClosedCampaignsList")]
        public async Task<ActionResult<List<Campaign>>> GetClosedCampaignsList()
        {
            DateTime todaysdate = DateTime.Now;
            var model = await GetJsonDataFromExternalApi();
            var closedCampaignList = model.Where(a => a.endDate <= todaysdate || a.procuredAmount >= a.totalAmount).ToList();
            return closedCampaignList;
        }

        public async Task<List<Campaign>> GetJsonDataFromExternalApi()
        {
            List<Campaign> Campagins = new List<Campaign>();
            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync("https://testapi.donatekart.com/api/campaign"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Campagins = JsonConvert.DeserializeObject<List<Campaign>>(apiResponse);
                        return Campagins;
                    }
                    return null;
                }
            }
        }
    }
}