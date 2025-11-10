
(function ($) {
  const onlyDigits = s => s.replace(/\D/g, '');

  function formatCPF(value) {
    const d = onlyDigits(value).slice(0, 11);
    if (!d) return '';
    if (d.length <= 3) return d;
    if (d.length <= 6) return d.slice(0, 3) + '.' + d.slice(3);
    if (d.length <= 9) return d.slice(0, 3) + '.' + d.slice(3, 6) + '.' + d.slice(6);
    return d.slice(0, 3) + '.' + d.slice(3, 6) + '.' + d.slice(6, 9) + '-' + d.slice(9);
  }

  $(function () {
    $(document).on('input', 'input.cpf-mascara', function () {
      const $this = $(this);
      $this.val(formatCPF($this.val()));
    });

    $(document).on('paste', 'input.cpf-mascara', function (e) {
      e.preventDefault();
      const text = (e.originalEvent.clipboardData || window.clipboardData).getData('text');
      $(this).val(formatCPF(text));
    });

    // ao submeter, remove máscara (apenas dígitos)
    $(document).on('submit', 'form', function () {
      $(this).find('input.cpf-mascara').each(function () {
        $(this).val(onlyDigits($(this).val()).slice(0, 11));
      });
    });
  });
})(jQuery);

