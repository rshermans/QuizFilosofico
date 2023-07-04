// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

<script>
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
    return new bootstrap.Popover(popoverTriggerEl)
})

 
    var tempoLimite = 60; // Tempo limite em segundos (por exemplo, 60 segundos)

    function iniciarContagemRegressiva() {
  var tempoRestante = tempoLimite;
  var elementoTempo = document.getElementById("tempo");

  var contador = setInterval(function() {
    tempoRestante--;
    elementoTempo.innerText = tempoRestante;

    if (tempoRestante <= 0) {
      clearInterval(contador);
      // Aqui você pode adicionar lógica para lidar com o tempo limite atingido, como enviar o formulário automaticamente ou exibir uma mensagem de aviso.
    }
  }, 1000); // A cada 1 segundo
}

    window.onload = function () {
        iniciarContagemRegressiva();
    };

</script>
