using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentManager.MVC.Data;
using StudentManager.MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.MVC.Controllers
{
    public class StudentViewModelController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiBaseUrl;

        public StudentViewModelController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }
        //private async Task<HttpClient> GetClientWithTokenAsync()
        //{
        //    var client = _clientFactory.CreateClient();

        //    // Lấy access token đã được lưu trong cookie khi người dùng đăng nhập thành công
        //    var accessToken = await HttpContext.GetTokenAsync("access_token");

        //    // Gắn token vào header của mỗi request HTTP gửi đi
        //    // API Server sẽ đọc header này để xác thực người dùng
        //    client.DefaultRequestHeaders.Authorization =
        //        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        //    return client;
        //}

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Students");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(jsonString);
                return View(students);
            }
            return View(new List<StudentViewModel>());
        }
        // Get: StudentViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Students/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var jsonString = await response.Content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<StudentViewModel>(jsonString);
            if (student == null)
            {
                return NotFound();
            }
            await LoadClassesIntoViewBag();
            return View(student);
        }
        private async Task LoadClassesIntoViewBag()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Classes");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(jsonString);
                ViewBag.Classes = new SelectList(classes, "Id", "ClassName");
            }
            else
            {
                ViewBag.Classes = new SelectList(new List<ClassViewModel>(), "Id", "ClassName");
            }
        }
        // GET: StudentViewModels/Create
        public async Task<IActionResult> Create()
        {
            await LoadClassesIntoViewBag();
            return View();
        }

        // POST: StudentViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var studentToSend = new
                {
                    FullName = student.FullName,
                    Age = student.Age,
                    ClassId = student.ClassId
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(studentToSend), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_apiBaseUrl}/api/Students", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "An error occurred: {await response.Content.ReadAsStringAsync()}");
            }   
            await LoadClassesIntoViewBag();
            return View(student);
        }

        // GET: StudentViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Students/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var jsonString = await response.Content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<StudentViewModel>(jsonString);
            if (student == null)
            {
                return NotFound();
            }
            await LoadClassesIntoViewBag();
            return View(student);
        }

        // POST: StudentViewModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel student)
        {
            if (id != student.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var client =  _clientFactory.CreateClient();

                // TẠO MỘT ĐỐI TƯỢNG "SẠCH" ĐỂ GỬI ĐI
                var studentToSend = new
                {
                    Id = student.Id, // Thêm Id cho việc update
                    FullName = student.FullName,
                    Age = student.Age,
                    ClassId = student.ClassId
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(studentToSend), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{_apiBaseUrl}/api/students/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, $"An error occurred: {await response.Content.ReadAsStringAsync()}");
            }

            // ... Tải lại dữ liệu nếu lỗi ...
            return View(student);
        }

        // GET: StudentViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Students/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var jsonString = await response.Content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<StudentViewModel>(jsonString);
            if (student == null)
            {
                return NotFound();
            }
            await LoadClassesIntoViewBag();
            return View(student);
        }

        // POST: StudentViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Students/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Unable to delete student. Please try again.");
            await LoadClassesIntoViewBag();
            return View();
        }

    }
}
