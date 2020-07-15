<template>
  <div id="minkontotemplate">
    <h1>Velkommen {{ user.navn }}!</h1>
    <div id="konto">
      <h3>Ret dine brugeroplysninger</h3>
      <form id="patchkonto" method="POST">
        <div class="formfelt">
          <label for="navn">Dit Navn</label>
          <input v-model="patchinfo.navn" name="navn" id="navn" />
        </div>
        <div class="formfelt">
          <label for="kodeord">Kodeord</label>
          <input v-model="patchinfo.kodeord" name="kodeord" id="kodeord" :type="viskode" />
        </div>
        <div class="formfelt">
          <label for="gkodeord">Gentag</label>
          <input v-model="gkodeord" name="gkodeord" id="gkodeord" :type="viskode" />
        </div>
      </form>Vis Kode
      <input type="checkbox" @input="toggleKode" />
      <button type="button" id="patch" form="patchkonto" @click="UserPatch()">Opdater</button>
    </div>
    <div id="upload">
      <h3>Upload et billede til din profil</h3>
      <form id="uploadform">
        <input ref="pickfile" id="fileinput" type="file" accept=".jpg, .jpeg, .png" @change="onFileChange" />
        <button
          type="button"
          id="fileinputknap"
          @click="$refs.pickfile.click()"
        >Vælg fil på computer</button>
        <button type="button" id="uploadknap" @click="UploadBillede()">Upload</button>
        <div id="preview">
          <p v-if="selectedFile">Her er det billede du er ved at uploade:</p>
          <div id="previewbox" v-if="selectedFile">
            <img id="previewimage" v-if="selectedFile" :src="selectedFile.fileurl" />
          </div>
        </div>
      </form>
    </div>
    <hr />
    <div id="billeder">
      <h2> Dine billeder </h2>
      <div class="billedbox" :id="image" v-for="image in images.slice().reverse()" :key="image">
        <img id="billede" :src="image" />
        <button type="button" @click="sletBillede(image)">
          <img id="remove" src="../assets/iconmonstr-x.png" /> Slet Billede
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import path from "path";

import { authentication } from "../_store";
import { router } from "../_helpers";
import { userService } from "../_services";
import { stringify } from "querystring";

let user = JSON.parse(localStorage.getItem("user"));
let MedlemsId = user.medlemsId;
let usertoken = "Bearer " + user.token;
let userurl = "http://localhost:51647/api/medlemmer/" + MedlemsId;
let uploadUrl = "http://localhost:51647/api/medlemmer/upload/" + MedlemsId;
var imageUrl = "http://localhost:51647/api/medlemmer/images/" + MedlemsId;

export default {
  data() {
    return {
      selectedFile: null,
      images: [],
      patchinfo: {
        navn: "",
        kodeord: ""
      },
      gkodeord: "",
      viskode: "password"
    };
  },
  computed: {
    user() {
      return this.$store.state.authentication.user;
    }
  },
  async created() {
    while(true) {
      const response = await fetch(imageUrl);
      const json = await response.json();
      this.images = json;    
    }
  },
  methods: {
    /* Account Management */
    UserPatch() {
      var pnavn = this.patchinfo.navn;
      var pkode = this.patchinfo.kodeord;
      var gkode = this.gkodeord;

      const { dispatch } = this.$store;

      if (pnavn != "" && pkode != "") {
        if (pkode === gkode) {
          this.patchUser();
        } else {
          alert("Begge kodeord skal være ens");
        }
      } else if (pnavn === "" && pkode != "") {
        if (pkode === gkode) {
          this.patchUserKode();
        } else {
          alert("Begge kodeord skal være ens");
        }
      } else if (pnavn != "" && pkode === "") {
        this.patchUserNavn();
      } else {
        alert(
          "Venligst udfyld mindst en af felterne før du prøver at rette dine oplysninger."
        );
      }
      /* Clear update form on submit */
      document.getElementById("patchkonto").reset();
    },

    toggleKode() {
      if (this.viskode === "password") {
        this.viskode = "text";
      } else {
        this.viskode = "password";
      }
    },

    patchUser() {
      const { navn } = this.patchinfo.navn;
      const { kodeord } = this.patchinfo.kodeord;

      var myHeaders = new Headers();
      myHeaders.append("Content-Type", "application/json");
      myHeaders.append("Authorization", usertoken);

      var raw = JSON.stringify([
        {
          op: "replace",
          path: "navn",
          value: this.patchinfo.navn
        },
        {
          op: "replace",
          path: "kodeord",
          value: this.patchinfo.kodeord
        }
      ]);

      var requestOptions = {
        method: "PATCH",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
      };

      fetch(userurl, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log("error", error));
    },

    patchUserNavn() {
      const { navn } = this.patchinfo.navn;

      var myHeaders = new Headers();
      myHeaders.append("Content-Type", "application/json");
      myHeaders.append("Authorization", usertoken);
      var raw = JSON.stringify([
        { op: "replace", path: "navn", value: this.patchinfo.navn }
      ]);

      var requestOptions = {
        method: "PATCH",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
      };

      fetch(userurl, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log("error", error));
    },

    patchUserKode() {
      var myHeaders = new Headers();
      myHeaders.append("Content-Type", "application/json");
      myHeaders.append("Authorization", usertoken);

      var raw = JSON.stringify([
        { op: "replace", path: "kodeord", value: this.patchinfo.kodeord }
      ]);

      var requestOptions = {
        method: "PATCH",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
      };

      fetch(userurl, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log("error", error));
    },
    /* Images */
    onFileChange(event) {
      this.selectedFile = event.target.files[0];
      this.selectedFile.fileurl = URL.createObjectURL(this.selectedFile);
    },
    UploadBillede() {
      let user = JSON.parse(localStorage.getItem("user"));
      let MedlemsId = user.medlemsId;
      let usertoken = "Bearer " + user.token;
      let userurl = "http://localhost:51647/api/medlemmer/upload/" + MedlemsId;

      // Data we're sending
      let data = new FormData();
      data.set("file", this.selectedFile, this.selectedFile.name);

      axios({
        method: "POST",
        url: userurl,
        data: data,
        config: {
          headers: {
            "Content-Type": "application/json",
            "Content-Type": "multipart/form-data",
            Authorization: usertoken
          }
        }
      });
      location.reload();
    },
    sletBillede(image) {
      var filename = path.basename(image);
      var deleteurl = imageUrl + "/" + filename;
      var imagebox = document.getElementById(image);

      var myHeaders = new Headers();
      myHeaders.append("Content-Type", "application/json");
      myHeaders.append("Authorization", usertoken);

      var requestOptions = {
        method: "DELETE",
        headers: myHeaders,
        redirect: "follow"
      };

      fetch(deleteurl, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log("error", error));
      imagebox.remove();
    }
  }
};
</script>

<style scoped>
hr {
  width: 95%;
  margin: 25px auto;
}

#konto,
#upload {
  display: inline-block;
  vertical-align: text-top;
  margin: 0 50px;
}

/* Patch Konto */

#patchkonto {
  display: table;
}

.formfelt {
  display: table-row;
}

.formfelt label {
  text-align: right;
  display: table-cell;
}

.formfelt input {
  display: table-cell;
  width: 200px;
  margin-bottom: 10px;
  margin-left: 10px;
  background-color: #cccccc;
  border: 1px solid grey;
}

#patch,
#uploadknap {
  background-color: rgb(0, 81, 255);
  color: white;
  border: none;
  padding: 5px 20px;
}

button:hover {
  cursor: pointer;
}

#patch {
  float: right;
}

/* Upload */
#upload {
  text-align: center;
}

input[type="file"] {
  display: none;
}

#fileinputknap {
  background-color: #cccccc;
  color: black;
  padding: 5px 20px;
  border: none;
}

#previewbox {
  width: 200px;
  margin: 0 auto;
}

#previewimage {
  width: 100%;
  border: 1px solid black;
}

.modal {
  display: none;
  position: fixed;
  z-index: 1;
  padding: 100px;
}

/* Billeder */
.billedbox {
  display: inline-block;
  position: relative;
  width: 200px;
  height: 150px;
  margin: 5px;
  border: 1px solid black;
  vertical-align: text-top;
  overflow: hidden;
}

#billede {
  max-width: 100%;
  height: auto;
}

.billedbox button {
  width: 100%;
  background-color: #cccccc;
  padding: 5px 0;
  border: none;
  position: absolute;
  bottom: 0;
  right: 0;
}

#remove {
  width: 15px !important;
  height: 15px !important;
  float: left;
  vertical-align: text-bottom;
  padding: 0 5px;
}
</style>