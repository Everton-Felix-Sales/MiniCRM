document.addEventListener("DOMContentLoaded", function () {

    // ==========================================
    // FUNÇÕES AUXILIARES DE MÁSCARA
    // ==========================================

    function aplicarMascaraDocumento(input) {
        let v = input.value.replace(/\D/g, '').slice(0, 14);
        if (v.length === 0) {
            input.value = '';
            return;
        }
        if (v.length <= 11) {
            // CPF: 000.000.000-00
            v = v.replace(/(\d{3})(\d)/, '$1.$2');
            v = v.replace(/(\d{3})(\d)/, '$1.$2');
            v = v.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
        } else {
            // CNPJ: 00.000.000/0000-00
            v = v.replace(/^(\d{2})(\d)/, '$1.$2');
            v = v.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
            v = v.replace(/\.(\d{3})(\d)/, '.$1/$2');
            v = v.replace(/(\d{4})(\d{1,2})$/, '$1-$2');
        }
        input.value = v;
    }

    function aplicarMascaraTelefone(input) {
        let v = input.value.replace(/\D/g, '').slice(0, 11);
        if (v.length === 0) {
            input.value = '';
        } else if (v.length <= 2) {
            input.value = `(${v}`;
        } else if (v.length <= 6) {
            input.value = `(${v.slice(0, 2)}) ${v.slice(2)}`;
        } else if (v.length <= 10) {
            input.value = `(${v.slice(0, 2)}) ${v.slice(2, 6)}-${v.slice(6)}`;
        } else {
            input.value = `(${v.slice(0, 2)}) ${v.slice(2, 7)}-${v.slice(7)}`;
        }
    }

    // ==========================================
    // 1. MÁSCARAS (TELEFONE E CPF/CNPJ)
    // ==========================================

    // Telefone / WhatsApp
    const inputsTelefone = document.querySelectorAll('input[name="Telefone"], input[id="Telefone"]');
    inputsTelefone.forEach(input => {
        aplicarMascaraTelefone(input); // Formata valor inicial se houver (ex: tela de Edição)
        input.addEventListener('input', () => aplicarMascaraTelefone(input));
    });

    // CPF / CNPJ
    const inputsDocumento = document.querySelectorAll('input[name="Documento"], input[id="Documento"]');
    inputsDocumento.forEach(input => {
        aplicarMascaraDocumento(input); // Formata valor inicial se houver (ex: tela de Edição)
        input.addEventListener('input', () => aplicarMascaraDocumento(input));
    });

    // ==========================================
    // 2. LIMPEZA DOS CARACTERES ANTES DE SALVAR
    // ==========================================
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function () {
            // Remove pontos, barras e hífen do campo Documento para enviar apenas números
            const docs = form.querySelectorAll('input[name="Documento"], input[id="Documento"]');
            docs.forEach(input => {
                input.value = input.value.replace(/\D/g, '');
            });
        });
    });

    // ==========================================
    // 3. MEDIDOR DE FORÇA DE SENHA
    // ==========================================
    const inputsSenha = document.querySelectorAll('input[type="password"]');

    inputsSenha.forEach(input => {
        const container = document.createElement('div');
        container.className = 'w3-margin-bottom';
        container.style.marginTop = '-8px';
        container.style.display = 'none';

        container.innerHTML = `
            <div class="w3-light-grey w3-round" style="height: 6px; overflow: hidden;">
                <div class="bar-forca w3-round" style="height: 100%; width: 0%; transition: width 0.3s, background-color 0.3s;"></div>
            </div>
            <span class="text-forca w3-tiny w3-text-grey" style="font-weight: bold; display: block; margin-top: 3px;"></span>
        `;

        if (input.parentNode) {
            input.parentNode.insertBefore(container, input.nextSibling);
        }

        const bar = container.querySelector('.bar-forca');
        const label = container.querySelector('.text-forca');

        input.addEventListener('input', function () {
            const senha = input.value;

            if (senha.length === 0) {
                container.style.display = 'none';
                return;
            }

            container.style.display = 'block';

            let pontuacao = 0;
            if (senha.length >= 6) pontuacao++;
            if (senha.length >= 8) pontuacao++;
            if (/[A-Z]/.test(senha) && /[a-z]/.test(senha)) pontuacao++;
            if (/[0-9]/.test(senha)) pontuacao++;
            if (/[^A-Za-z0-9]/.test(senha)) pontuacao++;

            if (pontuacao <= 2) {
                bar.style.width = '33%';
                bar.className = 'bar-forca w3-round w3-red';
                label.innerText = 'Força: Fraca';
                label.className = 'text-forca w3-tiny w3-text-red';
            } else if (pontuacao <= 4) {
                bar.style.width = '66%';
                bar.className = 'bar-forca w3-round w3-amber';
                label.innerText = 'Força: Média';
                label.className = 'text-forca w3-tiny w3-text-amber';
            } else {
                bar.style.width = '100%';
                bar.className = 'bar-forca w3-round w3-green';
                label.innerText = 'Força: Forte';
                label.className = 'text-forca w3-tiny w3-text-green';
            }
        });
    });

});