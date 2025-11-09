
$(document).ready(function () {

    $('#formBeneficiario').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/Beneficiario/Incluir",
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CPF": $(this).find("#Cpf").val(),
                "IDCLIENTE": obj.Id
            },
            error: function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success: function (r) {
                var $table = $('#tblBeneficiarios');
                console.warn("Tabela carregada");
                carregarBeneficiarios($table);
                console.warn("beneficiarios carregados");
                ModalDialog("Sucesso!", r);
                $("#formBeneficiario")[0].reset();
                $("#modalBeneficiario").modal('hide');

            }
        });
    });


    // Handler do botão "Alterar" (delegado)
    $(document).on('click', '#tblBeneficiarios .js-alterar', function () {
        var idBeneficiario = $(this).data('id');
        // Aqui você decide o que fazer:
        // 1) Abrir modal de edição
        // $('#meuModalEdicao').data('id', idBeneficiario).modal('show');
        // 2) Navegar para action Editar
        window.location.href = '/Beneficiario/Editar/' + encodeURIComponent(idBeneficiario);
    });

    // Inicialização no carregamento da página
    var $table = $('#tblBeneficiarios');
    if ($table.length) {
        carregarBeneficiarios($table);
    }

});

console.warn("script do beneficiário carregou");


// Utilitário simples para escapar HTML
function esc(str) {
    return String(str == null ? '' : str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

// Render de uma linha da tabela
function renderRow(item) {
    // Tenta cobrir JSON com PascalCase ou camelCase
    var id = item.Id != null ? item.Id : item.id;
    var nome = item.Nome != null ? item.Nome : item.nome;
    var cpf = item.Cpf != null ? item.Cpf : item.cpf;

    return [
        '<tr data-id="', esc(id), '">',
        '<td>', esc(nome || ''), '</td>',
        '<td>', esc(cpf || ''), '</td>',
        '<td>',
        '<button type="button" class="btn btn-xs btn-primary js-alterar" data-id="', esc(id), '">',
        'Alterar',
        '</button>',
        '</td>',
        '</tr>'
    ].join('');
}

// Popula a tabela
function popularTabela($tbody, lista) {
    if (!Array.isArray(lista) || lista.length === 0) {
        $tbody.html(
            '<tr><td colspan="3" class="text-center text-muted">Nenhum beneficiário encontrado.</td></tr>'
        );
        return;
    }

    var html = '';
    for (var i = 0; i < lista.length; i++) {
        html += renderRow(lista[i]);
    }
    $tbody.html(html);
}

// Carrega beneficiários via POST
function carregarBeneficiarios($table) {
    var $tbody = $table.find('tbody');

    idClienteEdicao = obj.Id;
    if (typeof idClienteEdicao === 'undefined' || idClienteEdicao === null || idClienteEdicao === '') {
        $tbody.html(
            '<tr><td colspan="3" class="text-center text-warning">Id do cliente não informado.</td></tr>'
        );
        return;
    }

    // Estado de carregando
    $tbody.html(
        '<tr><td colspan="3" class="text-center">Carregando...</td></tr>'
    );

    var url = '/Beneficiario/ListarPorCliente';

    $.ajax({
        url: url,
        type: 'POST',
        data: { idCliente: idClienteEdicao },
        dataType: 'json'
    })
        .done(function (dados) {
            var lista = Array.isArray(dados) ? dados : (dados.itens || []);
            popularTabela($tbody, lista);
        })
        .fail(function (xhr) {
            $tbody.html(
                '<tr><td colspan="3" class="text-center text-danger">Erro ao carregar beneficiários.</td></tr>'
            );
            console.error('Falha ao buscar beneficiários (POST):', xhr);
        });
}


// mesmo helper que você já usa
function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var html = '<div id="' + random + '" class="modal fade">' +
        '  <div class="modal-dialog">' +
        '    <div class="modal-content">' +
        '      <div class="modal-header">' +
        '        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
        '        <h4 class="modal-title">' + titulo + '</h4>' +
        '      </div>' +
        '      <div class="modal-body"><p>' + texto + '</p></div>' +
        '      <div class="modal-footer">' +
        '        <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>' +
        '      </div>' +
        '    </div>' +
        '  </div>' +
        '</div>';

    $('body').append(html);
    $('#' + random).modal('show');
}