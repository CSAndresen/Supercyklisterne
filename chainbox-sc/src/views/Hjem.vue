<template>
  <div>
    <h1> Velkommen til Supercyklisternes hjemmeside! </h1>
    <Slideshow></Slideshow>
    <Login v-if="user == null"></Login>
  </div>
</template>

<script>
import Login from "./Login";
import Slideshow from "../components/Slideshow";

export default {
  name: "hjem",
  computed: {
    user() {
      return this.$store.state.authentication.user;
    }
  },
  components: {
    Login,
    Slideshow
  }
};
</script>

<style scoped>
</style>