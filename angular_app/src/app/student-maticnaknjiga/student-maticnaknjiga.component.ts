import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";

declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css']
})
export class StudentMaticnaknjigaComponent implements OnInit {
  student_id: any;
  maticnaPodaci: any;
  noviSemestar: any;
  akademskeGodinePodaci: any;
  ovjeraZimskog: any;

  constructor(private httpKlijent: HttpClient, private route: ActivatedRoute) {}

  ovjeriLjetni(s:any) {

  }

  upisLjetni(s:any) {

  }


  ngOnInit(): void {
    this.route.params.subscribe(params=>{
      this.student_id=+params['id'];
    });
  this.fetchMaticnaKnjigaDetalji();
  this.fetchAkademskeGodine();
  }

  fetchAkademskeGodine()
  {
    this.httpKlijent.get(MojConfig.adresa_servera+"/AkademskeGodine/GetAll_ForCmb",MojConfig.http_opcije()).subscribe(x=>{
      this.akademskeGodinePodaci=x;
    });
  }
  fetchMaticnaKnjigaDetalji()
  {
    this.httpKlijent.get(`${MojConfig.adresa_servera}/MaticnaKnjiga/GetById/${this.student_id}`,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.maticnaPodaci=x;
    })
  }

  upisZimski() {
    this.noviSemestar={
      studentId : this.maticnaPodaci.student_id,
      datumUpisa:new Date,
      godinaStudija:1,
      akGodina_id:1,
      cijenaSkolarine:1000,
      obnova:false,
      napomena:""
    }
  }

  Snimi() {
    this.httpKlijent.post(MojConfig.adresa_servera+"/MaticnaKnjiga/Snimi",this.noviSemestar,MojConfig.http_opcije()).subscribe(x=>{
      this.fetchMaticnaKnjigaDetalji();
      this.noviSemestar=null;
    });
  }

  OvjeriSemestar() {
    this.httpKlijent.post(MojConfig.adresa_servera+"/MaticnaKnjiga/OvjeriSemestar",this.ovjeraZimskog,MojConfig.http_opcije()).subscribe(x=>{
      this.fetchMaticnaKnjigaDetalji();
      this.ovjeraZimskog=null;
    })
  }
}
