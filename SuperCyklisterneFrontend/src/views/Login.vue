<template>
  <div id="logintemplate">
    <div id="loginbox">
      <h3>Log ind her</h3>
      <form id="logindbruger" @submit.prevent="handleSubmit">
        <div class="formfelt">
          <label for="email">Email</label>
          <input v-model="email" name="email" id="email" type="email" required />
        </div>
        <div class="formfelt">
          <label for="kodeord">Kodeord</label>
          <input v-model="kodeord" name="kodeord" id="kodeord" :type="lviskode" required />
        </div>
      </form>Vis Kode
      <input type="checkbox" id="ltjekkode" @input="LtoggleKode" />
      <button type="submit" id="logind" form="logindbruger">Log ind</button>
    </div>
    <div id="createbox">
      <h3>Opret bruger her</h3>
      <form id="opretbruger">
        <div class="formfelt">
          <label for="cnavn">Dit Navn</label>
          <input v-model="opret.navn" name="cnavn" id="cnavn" type="text" required />
        </div>
        <div class="formfelt">
          <label for="cemail">Din Email</label>
          <input v-model="opret.email" id="cemail" name="cemail" type="email" pattern="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" required />
        </div>
        <div class="formfelt">
          <label for="ckodeord">Kodeord</label>
          <input
            v-model="opret.kodeord"
            name="ckodeord"
            id="ckodeord"
            class="kode"
            :type="cviskode"
            required
          />
        </div>
        <div class="formfelt">
          <label for="gkodeord">Gentag</label>
          <input
            v-model="opret.gkodeord"
            name="gkodeord"
            id="gkodeord"
            class="kode"
            :type="cviskode"
            required
          />
        </div>
      </form>Vis Kode
      <input type="checkbox" id="ctjekkode" @input="CtoggleKode" />
      <button type="button" id="opret" form="opretbruger" @click="MedlemOpret">Opret Bruger</button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { router } from "../_store";

export default {
  name: "login",
  data() {
    return {
      lviskode: "password",
      cviskode: "password",
      email: "",
      kodeord: "",
      opret: {
        email: "",
        kodeord: "",
        navn: ""
      },
      submitted: false
    };
  },
  computed: {
    loggingIn() {
      return this.$store.state.authentication.status.loggingIn;
    }
  },
  created() {
    // reset login status
    this.$store.dispatch("authentication/logout");
  },
  methods: {
    LtoggleKode() {
      if (this.lviskode === "password") {
        this.lviskode = "text";
      } else {
        this.lviskode = "password";
      }
    },
    handleSubmit(e) {
      this.submitted = true;
      const { email, kodeord } = this;
      const { dispatch } = this.$store;
      if (email && kodeord) {
        dispatch("authentication/login", { email, kodeord });
      }
      document.getElementById("logindbruger").reset();
    },
    CtoggleKode() {
      if (this.cviskode === "password") {
        this.cviskode = "text";
      } else {
        this.cviskode = "password";
      }
    },
    MedlemOpret() {
      if (this.opret.kodeord != this.opret.gkodeord) {
        alert("Begge kodeord skal være ens");
      } else if (this.opret.kodeord === this.opret.gkodeord) {
        axios.post("http://localhost:51647/api/medlemmer", {
          email: this.opret.email,
          kodeord: this.opret.kodeord,
          navn: this.opret.navn
        });
        document.getElementById("opretbruger").reset();
      }
    }
  }
};
</script>

<style scoped>
#loginbox,
#createbox {
  display: inline-block;
  vertical-align: text-top;
  margin: 0 50px;
}

form {
  display: table;
}

.formfelt {
  display: table-row;
}

label {
  text-align: right;
  display: table-cell;
}

.formfelt input {
  display: table-cell;
  width: 200px;
  margin-bottom: 10px;
  margin-left: 10px;
  background-color: lightgrey;
  border: 1px solid grey;
}

#opret,
#logind {
  background-color: rgb(0, 81, 255);
  color: white;
  border: none;
  padding: 5px;
  float: right;
}
</style>