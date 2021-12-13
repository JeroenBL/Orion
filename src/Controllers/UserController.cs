using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.Models;
using Orion.Services;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net.Mime;

namespace Orion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDataAccessService _dataAccesService;
        IDbConnection _sqliteConnection;

        public UserController(IDataAccessService dataAccessService)
        {
            _dataAccesService = dataAccessService;
            _sqliteConnection = _dataAccesService.LoadConnectionString();
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <List<UserModel>> Get()
        {
            using (_sqliteConnection)
            {
                var output = _sqliteConnection.Query<UserModel>("SELECT * FROM users", new DynamicParameters());
                return output.ToList();
            }
        }

        // GET: api/<UserController>
        [HttpGet("{externalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserModel>> Get(string externalId)
        {
            using (_sqliteConnection)
            {
                var result = _sqliteConnection.Query<UserModel>($"SELECT * FROM 'users' WHERE ExternalId = '{externalId}'", new DynamicParameters()).ToList();
                if (result.Count <= 0)
                {
                    return NotFound($"User with externalId {externalId} could not be found");
                }
                else
                {
                    return Ok(result[0]);
                }
            }
        }

        // POST api/<UserController>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult Post([FromBody] UserModel user)
        {
            try
            {
                using (_sqliteConnection)
                {
                    string query = $"INSERT INTO 'users' (ExternalId, FirstName, FamilyName, EmailAddress, PhoneNumber, Title, Description) " +
                            "values (@ExternalId, @FirstName, @FamilyName, @EmailAddress, @PhoneNumber, @Title, @Description)";
                    _sqliteConnection.Execute(query, user);
                    var result = _sqliteConnection.Query<UserModel>($"SELECT * FROM 'users' WHERE ExternalId = '{user.ExternalId}'", new DynamicParameters()).ToList();
                    return Ok(result[0]);
                }
            }
            catch (SQLiteException sqliteEx)
            {
                return BadRequest(sqliteEx.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost("Permission")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult Post([FromBody] PermissionModel permission)
        {
            try
            {
                using (_sqliteConnection)
                {
                    string query = $"INSERT INTO 'permissions' (DisplayName, UserId) " +
                            "values (@DisplayName, @UserId)";
                    _sqliteConnection.Execute(query, permission);
                    var result = _sqliteConnection.Query<PermissionModel>($"SELECT * FROM 'permissions' WHERE displayName = '{permission.DisplayName}'", new DynamicParameters()).ToList();
                    return Ok(result[0]);
                }
            }
            catch (SQLiteException sqliteEx)
            {
                return BadRequest(sqliteEx.Message);
            }
        }

        // Delete api/<UserController>
        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult Delete(string id)
        {
            try
            {
                using (_sqliteConnection)
                {
                    string query = $"Delete FROM 'users' WHERE Id = '{id}'";
                    _sqliteConnection.Execute(query);
                    return Ok();
                }
            }
            catch (SQLiteException sqliteEx)
            {
                return BadRequest(sqliteEx.Message);
            }
        }
    }
}
