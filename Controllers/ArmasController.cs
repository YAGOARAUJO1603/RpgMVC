using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RpgMvc.Models;

namespace RpgMvc.Controllers
{
    public class ArmasController : Controller
    {
        public string uriBase = "http://Hugo-DS.somee.com/RpgApi/Armas/";


         [HttpGet("Armas/{id}")]
        public async Task<ActionResult> IndexAsync(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<ArmaViewModel> lista = await Task.Run(() =>
                    JsonConvert.DeserializeObject<List<ArmaViewModel>>(serialized));

                    return View(lista);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpGet("Delete/{armaId}/{personagemId}")]
        public async Task<ActionResult> DeleteAsync(int armaId, int personagemId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "DeleteArma";
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ArmaViewModel ph = new ArmaViewModel();
                ph.ArmaId = armaId;
                ph.PersonagemId = personagemId;

                var content = new StringContent(JsonConvert.SerializeObject(ph));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    TempData["Mensagem"] = "Arma removida com sucesso";
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
            }
            return RedirectToAction("Index", new { Id = armaId });
        }

        [HttpGet]

        public async Task<ActionResult> CreateAsync(int id, string nome)
        {
            try
            {
                string uriComplementar = "GetArmas";
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);

                string serialized = await response.Content.ReadAsStringAsync();
                List<ArmaViewModel> armas = await Task.Run(() =>
                JsonConvert.DeserializeObject<List<ArmaViewModel>>(serialized));
                ViewBag.ListaArmas = armas;

                ArmaViewModel ph = new ArmaViewModel();
                ph.Personagem = new PersonagemViewModel();
                ph.Arma = new ArmaViewModel();
                ph.PersonagemId = id;
                ph.Personagem.Nome = nome;

                return View(ph);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Create", new { id, nome });
            }

        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ArmaViewModel ph)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(ph));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    TempData["Mensagem"] = "Arma cadastrada com sucesso";

                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index", new { id = ph.PersonagemId });
            }
            return RedirectToAction("Index", new { id = ph.PersonagemId });
        }

















    }
}