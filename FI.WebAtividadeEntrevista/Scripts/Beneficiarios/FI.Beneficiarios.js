
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
                carregarBeneficiarios($table);
                ModalDialog("Sucesso!", r);
                $("#formBeneficiario")[0].reset();
                $("#modalBeneficiario").modal('hide');

            }
        });
    });


    // Handler do botão "Alterar" (delegado)
    $(document).on('click', '#tblBeneficiarios .js-alterar', function () {
        var idBeneficiario = $(this).data('id');
        abrirModalEdicao(idBeneficiario);
    });

    // Inicialização no carregamento da página
    carregarBeneficiarios();

    $('#formEditarBeneficiario').on('submit', function (e) {
        e.preventDefault();

        var $btn = $('#btnSalvarEdicao');
        $btn.prop('disabled', true).data('old-text', $btn.text()).text('Salvando...');

        var payload = {
            Id: $('#BeneficiarioIdEdt').val(),
            Nome: $('#NomeEdt').val(),
            Cpf: $('#CpfEdt').val()
            // Se a sua API exigir IdCliente na alteração, inclua:
            // IdCliente: (typeof obj !== 'undefined' && obj) ? obj.Id : null
        };

        $.ajax({
            url: '/Beneficiario/Alterar',
            type: 'POST',
            data: payload,
            dataType: 'json'
        })
            .done(function (r) {
                // Recarrega a tabela após atualizar
                carregarBeneficiarios();

                ModalDialog('Sucesso!', r || 'Beneficiário atualizado com sucesso.');
                $('#modalEditarBeneficiario').modal('hide');
            })
            .fail(function (xhr) {
                if (xhr.status === 400 && xhr.responseJSON) {
                    ModalDialog('Atenção', xhr.responseJSON);
                } else if (xhr.status === 500) {
                    ModalDialog('Erro', 'Ocorreu um erro interno no servidor.');
                } else {
                    ModalDialog('Erro', 'Não foi possível salvar a alteração.');
                }
                console.error('Falha ao alterar beneficiário:', xhr);
            })
            .always(function () {
                $btn.prop('disabled', false).text($btn.data('old-text'));
            });
    });
});

function abrirModalEdicao(idBeneficiario) {
    // Reset de estado
    $('#formEditarBeneficiario')[0].reset();
    $('#BeneficiarioIdEdt').val('');
    $('#btnSalvarEdicao').prop('disabled', true);
    $('#editarLoading').show();

    // Abre modal imediatamente (com "Carregando..."), depois preenche
    $('#modalEditarBeneficiario').modal('show');

    // Endpoint que retorna os dados do beneficiário por ID
    // Ajuste se seu endpoint tiver outro nome/caminho ou parâmetro:
    $.ajax({
        url: '/Beneficiario/Obter',
        type: 'POST',
        data: { id: idBeneficiario },
        dataType: 'json'
    })
        .done(function (dados) {
            // Tenta cobrir PascalCase/camelCase
            var id = dados.Id != null ? dados.Id : dados.id;
            var nome = dados.Nome != null ? dados.Nome : dados.nome;
            var cpf = dados.Cpf != null ? dados.Cpf : dados.cpf;

            $('#BeneficiarioIdEdt').val(id || '');
            $('#NomeEdt').val(nome || '');
            $('#CpfEdt').val(cpf || '');

            $('#btnSalvarEdicao').prop('disabled', false);
        })
        .fail(function (xhr) {
            ModalDialog('Erro', 'Não foi possível carregar os dados do beneficiário.');
            $('#modalEditarBeneficiario').modal('hide');
            console.error('Falha ao obter beneficiário:', xhr);
        })
        .always(function () {
            $('#editarLoading').hide();
        });
}

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
function carregarBeneficiarios() {
    var $table = $('#tblBeneficiarios');
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