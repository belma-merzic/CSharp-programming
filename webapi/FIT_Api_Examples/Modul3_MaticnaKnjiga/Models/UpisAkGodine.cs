using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul2.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Modul3_MaticnaKnjiga.Models
{
  public class UpisAkGodine
  {
    [Key]
    public int id { get; set; }
    public DateTime datum_upisa_zimski { get; set; }
    public int godina_studija { get; set; }
    [ForeignKey(nameof(akGodina))]
    public int akGodina_id { get; set; }
    public AkademskaGodina akGodina { get; set; }
    public float cijena_skolarine { get; set; }
    public bool obnova { get; set; }
    public DateTime? datum_ovjere_zimski { get; set; }
    public string? napomena { get; set; }
    [ForeignKey(nameof(evidentiraoKorisnik))]
    public int evidentiraoKorisnik_id { get; set; }
    public KorisnickiNalog evidentiraoKorisnik { get; set; }
    [ForeignKey(nameof(student))]
    public int student_id { get; set; }
    public Student student { get; set; }
  }
}
