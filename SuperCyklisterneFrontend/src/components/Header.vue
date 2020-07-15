<template>
  <header>
    <a id="logo"> Super Cyklisterne </a>
    <router-link to="/hjem"> Hjem </router-link>
    <router-link to="/medlemmer"> Medlemmer </router-link>
    <router-link to="/omklubben"> Om Klubben </router-link>
    <router-link v-if="user == null" to="/login"> Medlemsområde </router-link>
    <router-link v-if="user != null" to="/minkonto"> Medlemsområde </router-link>
    <router-link v-if="user != null" to="/login" id="logud"> Logud </router-link>
  </header>
</template>

<script>
export default {
    name: "Header",
    computed: {
        user() {
            return this.$store.state.authentication.user;
        }
    }
}
</script>

<style scoped>
form {
    display: inline-block;
}
.router-link-active {
    font-weight: bold;
    border: none;
}

header, header a {
    margin: 0;
}

header {
    width: 100%;
    border-bottom: 5px solid #ffd633;
}

#logo {
    text-align: center;
    width: 75px;
    padding: 5px 20px;  
    margin: 0;
    background-color: #ffd633;
    font-weight: bolder;
    font-family: "Comic Sans MS", cursive, sans-serif;
}

a {
    display: inline-block;
    vertical-align: text-top;
    padding: 20px;
    text-decoration: none;
    color: black;
}

#logud {
    float: right;
    background-color: blue;
    color: white;
    vertical-align: text-top;
    padding: 5px 10px;
    margin: 12px;
    width: 100px;
    text-align: center;
}

</style>