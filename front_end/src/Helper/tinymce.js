export default {
  loadTinyMCE() {
    const script = document.createElement('script')
    script.src = '../../public/tinymce/js/tinymce/tinymce.min.js' // Đảm bảo đường dẫn chính xác
    script.onload = () => {
      console.log('TinyMCE script loaded')
      tinymce.init({
        selector: 'textarea',
        license_key: 'gpl',
        Min_height: 1000,
        plugins:
          'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount powerpaste  mentions  footnotes typography inlinecss markdown ',
        toolbar:
          'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image table | addcomment showcomments | a11ycheck typography | align lineheight |  numlist bullist indent outdent | emoticons charmap | removeformat',
        images_upload_url: 'https://localhost:7188/api/Cloud/uploadTinySingle',
        branding: false, // Removes "Powered by TinyMCE"
        stateless: true
      })
    }
    script.onerror = () => {
      console.error('Error loading the TinyMCE script')
    }
    document.head.appendChild(script)
  }
}
