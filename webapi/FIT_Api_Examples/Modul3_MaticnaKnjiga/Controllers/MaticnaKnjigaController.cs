using FIT_Api_Examples.Data;
using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FIT_Api_Examples.Modul3_MaticnaKnjiga.Controllers
{
  [Route("[controller]/[action]")]
  [ApiController]
  public class MaticnaKnjigaController : ControllerBase
  {
    private readonly ApplicationDbContext _dbContext;

    public MaticnaKnjigaController(ApplicationDbContext dbContext)
    {
      this._dbContext = dbContext;
    }

    [HttpGet("{studentid}")]
    public ActionResult GetById(int studentid)
    {
      var listaUpisi = _dbContext.UpisAkGodine
        .Where(s => s.student_id == studentid)
        .Select(s => new
        {
          s.id,
          akGodina_opis = s.akGodina.opis,
          s.godina_studija,
          s.obnova,
          s.datum_upisa_zimski,
          s.datum_ovjere_zimski,
          evidentirao_korisnik = s.evidentiraoKorisnik.korisnickoIme,
          s.napomena
        });

      var data = _dbContext.Student
        .Where(s => s.id == studentid)
        .Select(s => new
        {
          student_id = s.id,
          ime = s.ime,
          prezime = s.prezime,
          listaUpisi = listaUpisi.ToList()
        }).FirstOrDefault();

      return Ok(data);
    }

    public class UpisUZimskiVM
    {
      public int studentId { get; set; }
      public DateTime datumUpisa { get; set; }
      public int godinaStudija { get; set; }
      public int akGodina_id { get; set; }
      public float cijenaSkolarine { get; set; }
      public bool obnova { get; set; }
    }

    [HttpPost]
    public ActionResult Snimi([FromBody] UpisUZimskiVM x)
    {
      var novi = new UpisAkGodine();
      _dbContext.Add(novi);

      novi.student_id = x.studentId;
      novi.akGodina_id = x.akGodina_id;
      novi.godina_studija = x.godinaStudija;
      novi.obnova = x.obnova;
      novi.datum_upisa_zimski = x.datumUpisa;
      novi.cijena_skolarine = x.cijenaSkolarine;
      novi.evidentiraoKorisnik_id = 1;

      var lista = _dbContext.UpisAkGodine.Where(s => s.student_id == x.studentId).Select(g => g.godina_studija).ToList();
      if (lista.Contains(novi.godina_studija) && novi.obnova != true)
      {
        return BadRequest("Ne možete dodati istu godinu ukoliko nije obnova!");
      }

      _dbContext.SaveChanges();

      return Ok(novi);
    }

    public class OvjeriSemestarVM
    {
      public int id { get; set; }
      public int studentId { get; set; }
      public DateTime datumOvjere { get; set; }
      public string napomena { get; set; }
      public bool obnova { get; set; }
    }

    [HttpPost]
    public ActionResult OvjeriSemestar([FromBody] OvjeriSemestarVM x)
    {
      UpisAkGodine? godina = _dbContext.UpisAkGodine.FirstOrDefault(g=>g.id==x.id);

        godina.datum_ovjere_zimski = x.datumOvjere;
        godina.napomena = x.napomena;


      _dbContext.SaveChanges();

      return Ok(godina);
    }
  }
}

