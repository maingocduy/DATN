<template>
  <div class="shadow-lg rounded-lg overflow-hidden">
    <textarea id="editor" ref="editor"></textarea>
  </div>
</template>

<script>
export default {
  name: 'TinyMCEEditor',
  props: {
    modelValue: {
      type: String,
      default: ''
    }
  },

  watch: {
    modelValue(val) {
      if (this.editor) {
        this.editor.setContent(val)
      }
    }
  },
  mounted() {
    this.loadTinyMCE()
  },
  beforeDestroy() {
    if (this.editor) {
      this.editor.destroy()
    }
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
            'advlist anchor autolink charmap code codesample emoticons image link lists media nonbreaking searchreplace table visualblocks wordcount',
          toolbar:
            'undo redo | blocks fontfamily fontsize code nonbreaking | bold italic underline strikethrough | link image table | addcomment showcomments | a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
          images_upload_url: 'https://localhost:7188/api/Cloud/uploadTinySingle',
          advlist_bullet_styles: 'default,circle,disc,square',
          advlist_number_styles:
            'default,lower-alpha,lower-greek,lower-roman,upper-alpha,upper-roman',
          setup: (editor) => {
            this.editor = editor
            editor.on('init', () => {
              editor.setContent(this.modelValue)
            })
          }
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
