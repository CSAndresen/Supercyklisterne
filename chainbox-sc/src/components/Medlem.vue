<template>
<div id="kasse">
    <div id="medlem" v-for="(image, index) in images.slice().reverse()" :key="index">
        <img id="medlemsbillede" :src="image" :key="image" @click="vismodal(image)" />
        <div id="modal" @click="lukmodal" >
            <span id="close" @click="lukmodal"> &times; </span>
            <img id="modalimg" />
        </div>
    </div>
</div>
</template>

<script>
import axios from 'axios';


export default {
    name: "Medlem",
    props: {
        targetid: null
    },
    data() {
        return {
            images: [],
        }
    },
    mounted() {
        axios.get(`http://localhost:51647/api/medlemmer/images/${this.targetid}`).then(response => (this.images = response.data));
    },
    methods: {
        vismodal(image) {
            var modal = document.getElementById("modal");
            var modalimg = document.getElementById("modalimg");
            modal.style.display ="block";
            modalimg.setAttribute("src", image);
        },
        lukmodal() {
            var modal = document.getElementById("modal");
            modal.style.display ="none";
        },
    }
}
</script>

<style scoped>
#kasse {
    width: 100%;
}
#medlem {
    border: 1px solid black;
    margin: 0 5px;
    width: 160px;
    height: 100px;
    overflow: hidden;
    display: inline-block;
}

#medlemsbillede {
    width: 100%;
    height: 100%;
}

#medlemsbillede:hover {
    cursor: pointer;
}

#modal {
    display: none;
    position: fixed;
    z-index: 1;
    padding-top: 100px;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgba(0,0,0,0.9);
}

#modalimg {
    margin: auto;
    display: block;
    width: 80%;
    max-width: 800px
}

#close {
  position: absolute;
  top: 15px;
  right: 35px;
  color: #f1f1f1;
  font-size: 40px;
  font-weight: bold;
  transition: 0.3s;
}

#close:hover,
#close:focus {
  color: #bbb;
  text-decoration: none;
  cursor: pointer;
}
</style>