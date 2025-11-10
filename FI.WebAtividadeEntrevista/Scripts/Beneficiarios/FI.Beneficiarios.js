// Helpers simples e reaproveitáveis
function esc(str) {
    return String(str == null ? '' : str)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;')
        .replace(/>/g, '&gt;').replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

// Aceita PascalCase/camelCase no retorno
function normalizeBenef(b) {
    if (!b) return { Id: null, Nome: '', Cpf: '' };
    return {
        Id: b.Id != null ? b.Id : b.id,
        Nome: b.Nome != null ? b.Nome : b.nome,
        Cpf: b.Cpf != null ? b.Cpf : b.cpf
    };
}

// Render de uma linha
function renderRow(item) {
    var b = normalizeBenef(item);
    return [
        '<tr data-id="', esc(b.Id), '">',
        '<td>', esc(b.Nome || ''), '</td>',
        '<td>', esc(b.Cpf || ''), '</td>',
        '<td>',
        '  <button type="button" class="btn btn-xs btn-primary js-alterar" data-id="', esc(b.Id), '">Alterar</button> ',
        '  <button type="button" class="btn btn-xs btn-danger js-excluir" data-id="', esc(b.Id), '">Excluir</button>',
        '</td>',
        '</tr>'
    ].join('');
}

// Popular tabela
function popularTabela($tbody, lista) {
    if (!Array.isArray(lista) || !lista.length) {
        $tbody.html('<tr><td colspan="3" class="text-center text-muted">Nenhum beneficiário encontrado.</td></tr>');
        return;
    }
    var html = '';
    for (var i = 0; i < lista.length; i++) html += renderRow(lista[i]);
    $tbody.html(html);
}

// Mensagens de erro padronizadas
function handleAjaxError(xhr, fallbackMsg) {
    if (xhr && xhr.status === 400 && xhr.responseJSON) {
        ModalDialog('Atenção', xhr.responseJSON);
        return;
    }
    if (xhr && xhr.status === 500) {
        ModalDialog('Erro', 'Ocorreu um erro interno no servidor.');
        return;
    }
    ModalDialog('Erro', fallbackMsg || 'Não foi possível completar a operação.');
    if (window.console && console.error) console.error('AJAX error:', xhr);
}

// Carregar lista (POST)
function carregarBeneficiarios() {
    var $table = $('#tblBeneficiarios');
    if (!$table.length) return;

    var $tbody = $table.find('tbody');
    var idCliente = window.idClienteEdicao || (window.obj && window.obj.Id);

    if (!idCliente) {
        $tbody.html('<tr><td colspan="3" class="text-center text-warning">Id do cliente não informado.</td></tr>');
        return;
    }

    $tbody.html('<tr><td colspan="3" class="text-center">Carregando...</td></tr>');

    $.ajax({
        url: '/Beneficiario/ListarPorCliente',
        type: 'POST',
        dataType: 'json',
        data: { idCliente: idCliente }
        //, headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
    })
        .done(function (dados) {
            var lista = Array.isArray(dados) ? dados : (dados.itens || []);
            popularTabela($tbody, lista);
        })
        .fail(function (xhr) {
            $tbody.html('<tr><td colspan="3" class="text-center text-danger">Erro ao carregar beneficiários.</td></tr>');
            handleAjaxError(xhr);
        });
}

// Abrir modal de edição e preencher (POST)
function abrirModalEdicao(idBeneficiario) {
    $('#formEditarBeneficiario')[0].reset();
    $('#BeneficiarioIdEdt').val('');
    $('#btnSalvarEdicao').prop('disabled', true);
    $('#editarLoading').show();

    $('#modalEditarBeneficiario').modal('show');

    $.ajax({
        url: '/Beneficiario/Obter',
        type: 'POST',
        dataType: 'json',
        data: { id: idBeneficiario }
    })
        .done(function (dados) {
            var b = normalizeBenef(dados);
            $('#BeneficiarioIdEdt').val(b.Id || '');
            $('#NomeEdt').val(b.Nome || '');
            $('#CpfEdt').val(b.Cpf || '');
            $('#btnSalvarEdicao').prop('disabled', false);
        })
        .fail(function (xhr) {
            $('#modalEditarBeneficiario').modal('hide');
            handleAjaxError(xhr, 'Não foi possível carregar os dados do beneficiário.');
        })
        .always(function () {
            $('#editarLoading').hide();
        });
}

// Ready
$(function () {

    // Incluir (POST)
    $('#formBeneficiario').on('submit', function (e) {
        e.preventDefault();

        var idCliente = window.idClienteEdicao || (window.obj && window.obj.Id);
        if (!idCliente) {
            ModalDialog('Atenção', 'Id do cliente não informado.');
            return;
        }

        var payload = {
            Nome: $(this).find('#Nome').val(),
            Cpf: $(this).find('#Cpf').val(),
            IdCliente: idCliente
        };

        $.ajax({
            url: '/Beneficiario/Incluir',
            type: 'POST',
            dataType: 'json',
            data: payload
        })
            .done(function (r) {
                carregarBeneficiarios();
                ModalDialog('Sucesso!', r || 'Beneficiário incluído com sucesso.');
                $('#formBeneficiario')[0].reset();
                $('#modalBeneficiario').modal('hide');
            })
            .fail(function (xhr) {
                handleAjaxError(xhr, 'Não foi possível incluir o beneficiário.');
            });
    });

    // Clique em "Alterar" (delegado)
    $(document).on('click', '#tblBeneficiarios .js-alterar', function () {
        var idBeneficiario = $(this).data('id');
        abrirModalEdicao(idBeneficiario);
    });

    // Salvar edição (POST)
    $('#formEditarBeneficiario').on('submit', function (e) {
        e.preventDefault();

        var $btn = $('#btnSalvarEdicao');
        $btn.prop('disabled', true).data('old-text', $btn.text()).text('Salvando...');

        var payload = {
            Id: $('#BeneficiarioIdEdt').val(),
            Nome: $('#NomeEdt').val(),
            Cpf: $('#CpfEdt').val()
        };

        $.ajax({
            url: '/Beneficiario/Alterar',
            type: 'POST',
            dataType: 'json',
            data: payload
        })
            .done(function (r) {
                carregarBeneficiarios();
                $('#modalEditarBeneficiario').modal('hide');
            })
            .fail(function (xhr) {
                handleAjaxError(xhr, 'Não foi possível salvar a alteração.');
            })
            .always(function () {
                $btn.prop('disabled', false).text($btn.data('old-text'));
            });
    });

    // Clique em "Excluir" (delegado)
    $(document).on('click', '#tblBeneficiarios .js-excluir', function () {
        var id = $(this).data('id');
        if (!id) return;

        if (!confirm('Confirma a exclusão deste beneficiário?')) return;

        var $btn = $(this);
        var oldText = $btn.text();
        $btn.prop('disabled', true).text('Excluindo...');

        $.ajax({
            url: '/Beneficiario/Excluir/' + id,
            type: 'POST',
            dataType: 'json',
            data: { id: id }
        })
            .done(function (r) {
                carregarBeneficiarios();
            })
            .fail(function (xhr) {
                handleAjaxError(xhr, 'Não foi possível excluir o beneficiário.');
            })
            .always(function () {
                $btn.prop('disabled', false).text(oldText);
            });
    });

    // Carrega tabela ao abrir a tela (se existir)
    if ($('#tblBeneficiarios').length) {
        carregarBeneficiarios();
    }
});

// Mantido igual ao que você já usa
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
