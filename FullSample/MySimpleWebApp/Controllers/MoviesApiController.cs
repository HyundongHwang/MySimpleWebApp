using MySimpleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MySimpleWebApp.Controllers
{
    [RoutePrefix("api/movies")]
    public class MoviesApiController : ApiController
    {
        private mydbEntities _db = new mydbEntities();

        [Route("list")]
        public IHttpActionResult GetList()
        {
            return Ok(_db.Movies);
        }


        [Route("list2")]
        public List<Movie> GetList2()
        {
            return _db.Movies.ToList();
        }

        [Route("list3")]
        [Authorize]
        public List<Movie> GetList3()
        {
            return _db.Movies.ToList();
        }


        [Route("ex")]
        public IHttpActionResult GetEx()
        {
            throw new Exception("ex!!!");
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
