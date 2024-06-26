<script>
export default {
  name: 'TinyMCEEditor',
  mounted() {
    this.loadTinyMCE()
  },
  methods: {
    async loadTinyMCE() {
      const script = document.createElement('script')
      script.src = '../../public/tinymce/js/tinymce/tinymce.min.js' // Ensure the path is correct
      script.onload = () => {
        console.log('TinyMCE script loaded')
        tinymce.init({
          selector: 'textarea#editor',
          api_key: '9b2t0qnpq9lmj5ikt6n1a1br7o98vqwkzam8nzdmwxemt2g7', // Add your API key here
          license_key: 'gpl',
          height: 900,
          toolbar_sticky: true,
          plugins:
            'anchor autolink charmap codesample code advlist nonbreaking emoticons image link lists media searchreplace table visualblocks wordcount',
          toolbar:
            'undo redo | blocks fontfamily fontsize code nonbreaking | bold italic underline strikethrough | link image table | addcomment showcomments | a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
          images_upload_url: 'https://localhost:7188/api/Cloud/uploadTinySingle'
        })
      }
      script.onerror = () => {
        console.error('Error loading the TinyMCE script')
      }
      document.head.appendChild(script)
    }
  }
}
</script>

<template>
  <div class="shadow-lg rounded-lg overflow-hidden">
    <textarea id="editor" ref="editor"></textarea>
  </div>
</template>

<style scoped>
@media (min-width: 1024px) {
  #sample {
    display: flex;
    flex-direction: column;
    place-items: center;
    width: 100%;
  }
}
</style>
