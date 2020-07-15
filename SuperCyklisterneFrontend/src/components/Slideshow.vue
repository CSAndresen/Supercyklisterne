<template>
  <div id="slideshow">
    <swiper class="swiper" :options="swiperOption">
      <swiper-slide class="swiper-slide" v-for="image in images" :key="image">
        <img :src="image" />
      </swiper-slide>
    </swiper>
  </div>
</template>

<script>
import axios from "axios";
import { swiper, swiperSlide } from "vue-awesome-swiper";
import "swiper/dist/css/swiper.css";
import shuffle from "lodash/shuffle";

export default {
  data() {
    return {
      images: [],
      swiperOption: {
        slidesPerView: "auto",
        centeredSlides: true,
        spaceBetween: 30,
        autoplay: { delay: 2000 }
      }
    };
  },
  async created() {
    const response = await fetch("http://localhost:51647/api/medlemmer/images/alle");
    const json = await response.json();
    this.images = shuffle(json);
  },
  components: {
    swiper,
    swiperSlide
  }
};
</script>

<style scoped>
#slideshow {
  margin: 0 auto;
  width: 100%;
}

.swiper {
  margin: 0 auto;
  width: 100%;
  height: 500px;
}

.swiper-slide {
  background-color: black;
  width: 100%;
  height: auto;

  display: -webkit-box;
  display: -ms-flexbox;
  display: -webkit-flex;
  display: flex;
  -webkit-box-pack: center;
  -ms-flex-pack: center;
  -webkit-justify-content: center;
  justify-content: center;
  -webkit-box-align: center;
  -ms-flex-align: center;
  -webkit-align-items: center;
  align-items: center;
}

.swiper-slide img {
    height: auto;
    width: auto;
}
</style>