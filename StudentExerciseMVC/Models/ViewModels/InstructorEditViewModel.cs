using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using StudentExercisesAPI.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVC.Models.ViewModels
{
    public class InstructorEditViewModel
    {
        private readonly IConfiguration _config;

        public List<SelectListItem> Cohorts { get; set; }
        public Instructor instructor { get; set; }

        public InstructorEditViewModel() { }

        public InstructorEditViewModel(IConfiguration config)
        {
            using (IDbConnection conn = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            {
                Cohorts = conn.Query<Cohort>(@"
                    SELECT Id, Name FROM Cohort;
                   ")
                   .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.Id.ToString()
                }).ToList();
                ;
            }
        }
    }
}
