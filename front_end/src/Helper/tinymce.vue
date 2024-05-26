<script>
export default {
  name: 'TinyMCEEditor',
  mounted() {
    this.loadTinyMCE()
  },
  methods: {
    async loadTinyMCE() {
      const script = document.createElement('script')
      script.src = '../../public/tinymce/js/tinymce/tinymce.min.js' // Đảm bảo đường dẫn chính xác
      script.onload = () => {
        console.log('TinyMCE script loaded')
        tinymce.init({
          selector: 'textarea#editor',
          license_key: 'gpl',
          height: 900,

          toolbar_sticky: true,
          plugins:
            'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist casechange export formatpainter pageembed permanentpen powerpaste advtable advcode editimage advtemplate mentions tableofcontents footnotes typography inlinecss markdown ',
          toolbar:
            'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image table | addcomment showcomments | a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
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
