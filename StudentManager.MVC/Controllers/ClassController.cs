using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentManager.MVC.Data;
using StudentManager.MVC.ViewModel;

namespace StudentManager.MVC.Controllers
{
    public class ClassController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiBaseUrl;

        public ClassController(IHttpClientFactory clientFactory,IConfiguration configuration)
        {
            _clientFactory= clientFactory;
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }
          

        // GET: Class
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Classes");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(jsonString);
                return View(classes);
            }
            return View(new List<ClassViewModel>());
        }

        // GET: Class/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Classes/{id}");
            if(!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var classes = JsonConvert.DeserializeObject<ClassViewModel>(jsonString);
            if (classes == null)
            {
                return NotFound();
            }
            return View(classes);

        }

        // GET: Class/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassViewModel classViewModel)
        {
            if(ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var classtoSend = new
                {
                    ClassName = classViewModel.ClassName
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(classtoSend), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_apiBaseUrl}/api/Classes", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred: {await response.Content.ReadAsStringAsync()}");
                }
            }
           return View(classViewModel);
        }

        // GET: Class/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Classes/{id}");
            if(!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var classViewModel = JsonConvert.DeserializeObject<ClassViewModel>(jsonString);
            if (classViewModel == null)
            {
                return NotFound();
            }
            return View(classViewModel);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassViewModel classViewModel)
        {
            if (id != classViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var classtoSend = new
                {
                    Id = classViewModel.Id,
                    ClassName = classViewModel.ClassName
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(classtoSend), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{_apiBaseUrl}/api/Classes/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred: {await response.Content.ReadAsStringAsync()}");
                }
            }
            return View(classViewModel);
        }

        // GET: Class/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Classes/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var classViewModel = JsonConvert.DeserializeObject<ClassViewModel>(jsonString);
            if (classViewModel == null)
            {
                return NotFound();
            }
            return View(classViewModel);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Classes/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred: {await response.Content.ReadAsStringAsync()}");
                return View();
            }
        }        
    }
}
