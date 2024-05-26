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
