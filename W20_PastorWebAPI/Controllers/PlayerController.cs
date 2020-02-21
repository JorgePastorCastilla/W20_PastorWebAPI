using Dapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W20_PastorWebAPI.Models;

namespace W20_PastorWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {
        // POST api/Player/RegisterPlayer
        [HttpPost]
        [Route("RegisterPlayer")]
        public IHttpActionResult RegisterPlayer(PlayerModel player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = "INSERT INTO dbo.Players(Id, Name, Email, BirthDay) " +
                    $"VALUES('{player.Id}','{player.Name}','{player.Email}','{player.BirthDay}')";
                try
                {
                    cnn.Execute(sql);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error inserting player in database: " + ex.Message);
                }

                return Ok();
            }
        }

        // GET api/Player/Info
        [HttpGet]
        [Route("Info")]
        public PlayerModel GetPlayerInfo()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT Id, Name, Email, BirthDay FROM dbo.Players " +
                    $"WHERE Id LIKE '{authenticatedAspNetUserId}'";
                var player = cnn.Query<PlayerModel>(sql).FirstOrDefault();
                return player;
            }
        }

    }
}
