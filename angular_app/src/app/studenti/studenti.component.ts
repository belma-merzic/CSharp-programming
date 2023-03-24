import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Router} from "@angular/router";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css']
})
export class StudentiComponent implements OnInit {

  title:string = 'angularFIT2';
  ime_prezime:string = '';
  opstina: string = '';
  studentPodaci: any[]=[];
  filter_ime_prezime: boolean;
  filter_opstina: boolean;
  odabrani_student: any;
  opstinePodaci: any;

  constructor(private httpKlijent: HttpClient, private router: Router) {
  }

  testirajWebApi() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/GetAll", MojConfig.http_opcije()).subscribe((x:any)=>{
      this.studentPodaci = x;
    });
  }

  ngOnInit(): void {
    this.testirajWebApi();
    this.fetchOpstine();
  }

  get_podaci() {
    return this.studentPodaci.filter((s:any)=>
      (!this.filter_ime_prezime || (s.ime+" "+s.prezime).startsWith(this.ime_prezime) || (s.prezime+" "+s.ime).startsWith(this.ime_prezime))
      &&
      (!this.filter_opstina || s.opstina_rodjenja_opis.startsWith(this.opstina))
    )
  }

  ObrisiButton(s:any) {
    this.httpKlijent.post(`${MojConfig.adresa_servera}/Student/Obrisi/${s.id}`, MojConfig.http_opcije()).subscribe(x=>{
      this.testirajWebApi();
    });
  }

  fetchOpstine(){
    this.httpKlijent.get(MojConfig.adresa_servera+"/Opstina/GetByAll",MojConfig.http_opcije()).subscribe(x=>{
      this.opstinePodaci=x;
    })
  }

  noviStudent() {
    this.odabrani_student={
      id:0,
      ime:"",
      prezime:"",
      broj_indeksa:"",
      drzava_rodjenja:"",
      opstina_rodjenja_opis:"",
      opstina_rodjenja_id:2,
      datum_dodavanja:"",
      slika_korisnika_nova:""
    }
  }

  maticnaKnjigaDugme(s: any) {
    this.router.navigate(["/student-maticnaknjiga",s.id]);
  }

  Snimi() {
    this.httpKlijent.post(MojConfig.adresa_servera+"/Student/Snimi", this.odabrani_student,MojConfig.http_opcije()).subscribe(x=>{
      this.testirajWebApi();
      this.odabrani_student=null;
    });
  }

  get_slika_DB(s: any) {
    return `${MojConfig.adresa_servera}/Student/GetSlikaDB/${s.id}`;
  }

  get_slika_FS(s: any) {
    return `${MojConfig.adresa_servera}/Student/GetSlikaFS/${s.id}`;
  }

  generisi_preview() {
    // @ts-ignore
    var file = document.getElementById("slika-input").files[0];

    if(file)
    {
      var reader = new FileReader();
      let this2=this;

      reader.onload = function ()
      {
        this2.odabrani_student.slika_korisnika_nova = reader.result.toString();
      }
      reader.readAsDataURL(file);
    }
  }
}
