using Newtonsoft.Json;
using StudentMarksMgnt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace StudentMarksMgnt.Controllers
{
    [System.Web.Http.RoutePrefix("Students")]
    public class StudentsController : ApiController
    {

        [HttpGet]
        [Route("GetAllStudents")]
        public HttpResponseMessage GetAllStudents()
        {
            string path = HostingEnvironment.MapPath("~/Data/StudentMarks.json");

            var json = File.ReadAllText(path);
            var response = JsonConvert.DeserializeObject<ListofStudents>(json);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("AddStudent")]
        public HttpResponseMessage AddStudent(Student student)
        {
            string path = HostingEnvironment.MapPath("~/Data/StudentMarks.json");

            var json = File.ReadAllText(path);
            var response = JsonConvert.DeserializeObject<ListofStudents>(json);

            if (response != null)
            {
                var stud_validation = response.Students.Where(r => r.StudentID == student.StudentID)?.FirstOrDefault();
                if (stud_validation == null)
                {
                    response.Students.Add(student);
                    var output = new { Status = "SUCCESS", Message = "Student " + student.StudentID + " - " + student.StudentName + " Added Successfully ..." };

                    string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(response,
                               Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(path, newJsonResult);

                    return Request.CreateResponse(HttpStatusCode.OK, output);

                }
                else
                {
                    var output = new { Status = "INFO", Message = "Student " + student.StudentID + " - " + student.StudentName + " Already Exists ..." };
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("UpdateStudent")]
        public HttpResponseMessage UpdateStudent(Student student)
        {
            string path = HostingEnvironment.MapPath("~/Data/StudentMarks.json");

            var json = File.ReadAllText(path);
            var data_json = JsonConvert.DeserializeObject<ListofStudents>(json);

            if (data_json != null)
            {
                var data_to_be_update = data_json.Students.Where(r => r.StudentID == student.StudentID)?.FirstOrDefault();

                if (data_to_be_update != null)
                {
                    var new_data_json = data_json.Students.Where(r => r.StudentID != data_to_be_update.StudentID)?.ToList(); // excluded student has to be updated
                    data_to_be_update.Subject1 = student.Subject1;
                    data_to_be_update.Subject2 = student.Subject2;
                    data_to_be_update.Subject3 = student.Subject3;
                    data_to_be_update.Subject4 = student.Subject4;
                    data_to_be_update.Subject5 = student.Subject5;

                    new_data_json.Add(data_to_be_update); // Adding updated student from input 

                    var output = new { Status = "SUCCESS", Message = "Student " + student.StudentID + " - " + student.StudentName + " Updated Successfully ..." };

                    ListofStudents new_list_of_stud = new ListofStudents();
                    new_list_of_stud.Students = new_data_json;
                    string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(new_list_of_stud,
                               Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(path, newJsonResult);

                    return Request.CreateResponse(HttpStatusCode.OK, output);

                }
                else
                {
                    var output = new { Status = "ERROR", Message = "Student " + student.StudentID + " - does not exists ..." };
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("DeleteStudent")]
        public HttpResponseMessage DeleteStudent(Student student)
        {
            string path = HostingEnvironment.MapPath("~/Data/StudentMarks.json");

            var json = File.ReadAllText(path);
            var data_json = JsonConvert.DeserializeObject<ListofStudents>(json);

            if (data_json != null)
            {
                var data_to_be_update = data_json.Students.Where(r => r.StudentID == student.StudentID)?.FirstOrDefault();

                if (data_to_be_update != null)
                {
                    var new_data_json = data_json.Students.Where(r => r.StudentID != data_to_be_update.StudentID)?.ToList(); // excluded student has to be updated
                    var output = new { Status = "SUCCESS", Message = "Student " + student.StudentID + " - " + student.StudentName + " Deleted Successfully ..." };

                    ListofStudents new_list_of_stud = new ListofStudents();
                    new_list_of_stud.Students = new_data_json;
                    string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(new_list_of_stud,
                               Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(path, newJsonResult);

                    return Request.CreateResponse(HttpStatusCode.OK, output);

                }
                else
                {
                    var output = new { Status = "ERROR", Message = "Student " + student.StudentID + " - Does Not Exists ..." };
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

    }
}
