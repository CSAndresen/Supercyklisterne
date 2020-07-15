<template>
  <div id="app">
    <Header></Header>
    <Main></Main>
    <Footer></Footer>
  </div>
</template>

<script>
import Header from './components/Header'
import Main from './components/Main'
import Footer from './components/Footer'

var sitebody = document.getElementsByTagName("BODY")[0];

export default {
  name: "app",
  components: {
    Header,
    Main,
    Footer
  },
  mounted() {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
    const newColorScheme = e.matches ? "dark" : "light";
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      sitebody.style.backgroundColor = "rgb(25, 25, 25";
    } else {
      sitebody.style.backgroundColor = "rgb(225, 225, 225";
    }
    });
  },
  methods: {

  }
}
</script>

<style>
body {
  background-color: rgb(25,25,25);
  margin: 0;
}
#app {
  background-color: white;
  margin: 0 auto;
  width: 1000px;
}

@media only screen and (max-width: 999px) {
  #app {
    width: 100%;
  }
}
</style>