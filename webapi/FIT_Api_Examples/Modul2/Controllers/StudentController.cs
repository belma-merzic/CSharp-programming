using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
  //[Authorize]
  [ApiController]
  [Route("[controller]/[action]")]
  public class StudentController : ControllerBase
  {
    private readonly ApplicationDbContext _dbContext;

    public StudentController(ApplicationDbContext dbContext)
    {
      this._dbContext = dbContext;
    }

    public class StudentGetAllVM
    {
      public int id { get; set; }
      public string ime { get; set; }
      public string prezime { get; set; }
      public string broj_indeksa { get; set; }
      public string drzava_rodjenja { get; set; }
      public string? opstina_rodjenja_opis { get; set; }
      public int? opstina_rodjenja_id { get; set; }
      public string datum_dodavanja { get; set; }
      public string? slika_korisnika_nova { get; set; }
    }

    [HttpGet]
    public ActionResult<List<StudentGetAllVM>> GetAll(string? ime_prezime)
    {
      var data = _dbContext.Student
      .Include(s => s.opstina_rodjenja.drzava)
      .Select(s => new StudentGetAllVM
      {
        id = s.id,
        ime = s.ime,
        prezime = s.prezime,
        broj_indeksa = s.broj_indeksa,
        drzava_rodjenja = s.opstina_rodjenja.drzava.naziv,
        opstina_rodjenja_opis = s.opstina_rodjenja.description,
        opstina_rodjenja_id = s.opstina_rodjenja_id,
        datum_dodavanja = s.created_time.ToString("dd.MM.yyyy")
      }).ToList();

      return Ok(data);
    }

    [HttpPost]
    public ActionResult Snimi([FromBody] StudentGetAllVM x)
    {
      Student? student;
      if(x.id==0)
      {
        student = new Student()
        {
          created_time = DateTime.Now
        };
        _dbContext.Add(student);
      }
      else
      {
        student = _dbContext.Student.FirstOrDefault(s => s.id == x.id);
      }

      if (student == null)
        return BadRequest("Pogresan ID");

      student.ime = x.ime;
      student.prezime = x.prezime;
      student.broj_indeksa = x.broj_indeksa;
      student.opstina_rodjenja_id = x.opstina_rodjenja_id;

      if(!string.IsNullOrEmpty(x.slika_korisnika_nova))
      {
        //slika se snima u DB
        byte[]? slika_bajtovi = x.slika_korisnika_nova?.ParseToBytes();
        student.slika_korisnika_bajtovi = slika_bajtovi;

        if (slika_bajtovi == null)
          return BadRequest("Format slike nije Base64");

        //slika se snima na File System
        Fajlovi.Snimi(slika_bajtovi, "slike_korisnika/" + student.id);
      }

      _dbContext.SaveChanges();
      return Ok();
    }

    [HttpPost("{id}")]
    public ActionResult Obrisi(int id)
    {
      Student? student = _dbContext.Student.Find(id);

      if (student == null)
        return BadRequest("pogresan id");

      _dbContext.Remove(student);
      _dbContext.SaveChanges();
      return Ok(student);
    }

    [HttpGet("{id}")]
    public ActionResult GetSlikaDB(int id)
    {
      var student = _dbContext.Student.Find(id);
      byte[]? bajtovi_slike = student.slika_korisnika_bajtovi ?? Fajlovi.Ucitaj("wwwroot/profile_images/empty.png");

      if (bajtovi_slike == null)
        return BadRequest("Nema slike");

      return File(bajtovi_slike, "image/png");
    }

    [HttpGet("{id}")]
    public ActionResult GetSlikaFS(int id)
    {
      byte[]? bajtovi_slike = Fajlovi.Ucitaj("slike_korisnika/" + id) ?? Fajlovi.Ucitaj("wwwroot/profile_images/empty.png");

      if (bajtovi_slike == null)
        return BadRequest("Nema slike");

      return File(bajtovi_slike, "image/png");
    }
  }
}
