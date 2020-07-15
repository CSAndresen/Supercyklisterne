<template>
  <div id="medlemmer">
    <h1>Mød vores medlemmer!</h1>
    <p id="abc"> </p>
    <ul>
      <div class="medlemskasse" v-for="medlem in medlemmer" :key="medlem">
        <h3> {{ medlem.navn }} </h3>
        <hr>
        <Medlem :targetid="`${medlem.medlemsId}`" />
      </div>
    </ul>
  </div>
</template>

<script>
import axios from "axios";
import Medlem from "../components/Medlem";

export default {  
  name: "Medlemmer",
  data() {
    return {
      medlemmer: null
    }
  },
  mounted() {
    axios.get("http://localhost:51647/api/medlemmer/").then(response => (this.medlemmer = response.data));
  },
  components: {
    Medlem
  }
}
</script>

<style scoped>
.medlemskasse {
  display: inline-block;
  vertical-align: text-top;
  width: 90%;
  margin: 10px;
  padding: 0 10px;
  text-align: left;
}

.medlemskasse h3 {
  margin: 0;
  font-size: 30px;
}
</style>