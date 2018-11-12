using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models.ViewModels;
using StudentExercisesAPI.Data;

namespace StudentExerciseMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Instructors
        public async Task<ActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
                IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(@"
                    SELECT 
                        i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
                    FROM Instructor i
                ");
                return View(instructors);
            }
        }

        // GET: Instructors/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string sql = $@"
            SELECT 
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.Specialty,
                i.CohortId
             FROM Instructor i
             WHERE i.Id = {id} 
            ";

            using (IDbConnection conn = Connection)
            {
                Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
                return View(instructor);
            }
        }

        // GET: Instructors/CreateNewInstructor
        public ActionResult Create()
        {
            var model = new InstructorCreateViewModel(_config);
            return View(model);
        }

        // POST: Instructors/CreateNewInstructor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InstructorCreateViewModel model)
        {
            string sql = $@"INSERT INTO Instructor
            (FirstName, LastName, SlackHandle, CohortId)
            VALUES
            (
                '{model.instructor.FirstName}',
                '{model.instructor.LastName}',
                '{model.instructor.SlackHandle}',
                '{model.instructor.CohortId}'
            );";

            using (IDbConnection conn = Connection)
            {
                var newId = await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}