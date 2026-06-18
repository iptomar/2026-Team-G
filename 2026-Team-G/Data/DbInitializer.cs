using _2026_Team_G.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2026_Team_G.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Garante que a base de dados está criada
            context.Database.EnsureCreated();

            // Executa os métodos de Seed para cada formulário individualmente
            SeedRequerimentoDocumentos(context);
            SeedBoletimMatricula(context);

            // Guarda tudo na base de dados
            context.SaveChanges();
        }

        private static void SeedRequerimentoDocumentos(ApplicationDbContext context)
        {
            if (context.Formularios.Any(f => f.Title == "Requerimento - Emissão de Documentos (ACA 30 60-3)"))
                return;

            var campos = new List<FormFieldModel>();
            int orderGlobal = 0;

            void AdicionarCampo(string linha, string tipo, string label, bool obrigatorio = false)
            {
                campos.Add(new FormFieldModel
                {
                    FieldId = "field_" + Guid.NewGuid().ToString("N").Substring(0, 9),
                    Type = tipo,
                    Label = label,
                    IsRequired = obrigatorio,
                    Options = linha,
                    OrderIndex = orderGlobal++,
                    Width = "100%",
                    Placeholder = ""
                });
            }

            AdicionarCampo("0", "texto", "Escola (ESGT, ESTA ou ESTT)", true);
            AdicionarCampo("1", "title", "1. IDENTIFICAÇÃO");
            AdicionarCampo("2", "texto", "Nome completo", true);
            AdicionarCampo("2", "numero", "N.º de aluno", true);
            AdicionarCampo("3", "texto", "BI / CC / Outro", true);
            AdicionarCampo("3", "texto", "Validade do Documento");
            AdicionarCampo("4", "texto", "Curso", true);
            AdicionarCampo("5", "title", "2. CONTACTOS");
            AdicionarCampo("6", "texto", "Morada completa");
            AdicionarCampo("6", "texto", "Código postal");
            AdicionarCampo("7", "numero", "Telefone / Telemóvel");
            AdicionarCampo("7", "texto", "E-mail");
            AdicionarCampo("8", "title", "3. DOCUMENTO(S) REQUERIDO(S)");
            AdicionarCampo("9", "check box", "3.1 Certificado de matrícula, inscrição ou frequência");
            AdicionarCampo("9", "check box", "Com aproveitamento");
            AdicionarCampo("9", "check box", "Sem aproveitamento");
            AdicionarCampo("10", "texto", "Requer para efeitos de");
            AdicionarCampo("11", "check box", "3.2 Certificado de realização de Unidades Curriculares");
            AdicionarCampo("11", "check box", "3.2 Certificado de realização de Unidades Isoladas");
            AdicionarCampo("12", "texto", "3.3 Outros documentos");
            AdicionarCampo("13", "check box", "3.4 Carta de Curso");
            AdicionarCampo("13", "check box", "3.5 Diploma de Conclusão");
            AdicionarCampo("14", "texto", "Indique o Grau (Microcredenciação, CET, Licenciatura, etc)");
            AdicionarCampo("15", "check box", "Com discriminação das classificações obtidas");
            AdicionarCampo("15", "check box", "Sem discriminação das classificações obtidas");
            AdicionarCampo("16", "check box", "Em Português");
            AdicionarCampo("16", "check box", "Em Inglês");
            AdicionarCampo("17", "title", "ANEXO - Suplemento ao Diploma | Atividades Elegíveis");
            AdicionarCampo("18", "textarea", "Descreva as atividades elegíveis:");
            AdicionarCampo("19", "texto", "Data do Pedido (DD/MM/AAAA)", true);
            AdicionarCampo("19", "assinatura", "Assinatura do Requerente", true);

            var formulario = new Formulario
            {
                Title = "Requerimento - Emissão de Documentos (ACA 30 60-3)",
                Description = "Formulário oficial do Politécnico de Tomar para pedido de emissão de certificados, cartas de curso, diplomas e suplementos.",
                IsActive = true,
                Fields = campos
            };

            context.Formularios.Add(formulario);
        }
        private static void SeedBoletimMatricula(ApplicationDbContext context)
        {
            // Se já existir, cancela a operação para não duplicar
            if (context.Formularios.Any(f => f.Title == "Boletim de Matrícula (Mod. IPT/DSA 03.11-V1)"))
                return;

            var campos = new List<FormFieldModel>();
            int orderGlobal = 0;

            void AdicionarCampo(string linha, string tipo, string label, bool obrigatorio = false)
            {
                campos.Add(new FormFieldModel
                {
                    FieldId = "field_" + Guid.NewGuid().ToString("N").Substring(0, 9),
                    Type = tipo,
                    Label = label,
                    IsRequired = obrigatorio,
                    Options = linha,
                    OrderIndex = orderGlobal++,
                    Width = "100%",
                    Placeholder = ""
                });
            }

            // Cabeçalho
            AdicionarCampo("0", "texto", "Ano letivo", true);
            AdicionarCampo("0", "texto", "Escola (ESGT, ESTA, ESTT)", true);
            AdicionarCampo("1", "texto", "Curso", true);
            AdicionarCampo("1", "numero", "Nº de aluno", true);

            // 1. DADOS PESSOAIS
            AdicionarCampo("2", "title", "1. DADOS PESSOAIS");
            AdicionarCampo("3", "texto", "Nome completo", true);
            AdicionarCampo("4", "texto", "Nome do pai");
            AdicionarCampo("4", "texto", "Nome da mãe");
            AdicionarCampo("5", "texto", "Data de nascimento (DD/MM/AAAA)", true);
            AdicionarCampo("5", "texto", "Naturalidade");
            AdicionarCampo("6", "texto", "Nacionalidade");
            AdicionarCampo("6", "texto", "2ª Nacionalidade");

            AdicionarCampo("7", "label", "1.1 Sexo");
            AdicionarCampo("7", "check box", "Masculino");
            AdicionarCampo("7", "check box", "Feminino");

            AdicionarCampo("8", "label", "1.2 Estado Civil");
            AdicionarCampo("8", "check box", "Solteiro");
            AdicionarCampo("8", "check box", "Casado");
            AdicionarCampo("8", "check box", "Divorciado / Separado");
            AdicionarCampo("8", "check box", "Viúvo");

            AdicionarCampo("9", "numero", "Agregado Familiar (Nº pessoas)");
            AdicionarCampo("9", "numero", "Nº de irmãos");
            AdicionarCampo("9", "texto", "NIF", true);
            AdicionarCampo("10", "texto", "Bairro Fiscal");
            AdicionarCampo("10", "texto", "País (Fiscal)");

            AdicionarCampo("11", "label", "1.3 Identificação");
            AdicionarCampo("11", "texto", "Nº de Identificação", true);
            AdicionarCampo("11", "texto", "Data de Emissão");
            AdicionarCampo("11", "texto", "Data de Validade");
            AdicionarCampo("12", "texto", "Tipo de Documento (Cartão de Cidadão, BI, Passaporte, etc)");

            // 2. CONTACTOS
            AdicionarCampo("13", "title", "2. CONTACTOS");
            AdicionarCampo("14", "texto", "2.1 Morada em tempo de aulas (Endereço)", true);
            AdicionarCampo("15", "texto", "Código Postal");
            AdicionarCampo("15", "texto", "Freguesia");
            AdicionarCampo("15", "texto", "País");
            AdicionarCampo("16", "texto", "E-mail");
            AdicionarCampo("16", "numero", "Telefone / Telemóvel", true);
            AdicionarCampo("17", "check box switch", "Deslocado da residência permanente?");
            AdicionarCampo("18", "texto", "2.2 Outros contactos (Morada secundária)");
            AdicionarCampo("19", "check box", "Morada de correspondência: Principal");
            AdicionarCampo("19", "check box", "Morada de correspondência: Secundária");

            // 3. DADOS DE INGRESSO
            AdicionarCampo("20", "title", "3. DADOS DE INGRESSO");
            AdicionarCampo("21", "label", "Regime de ingresso");
            AdicionarCampo("21", "check box", "Concurso Nacional (1ª/2ª/3ª Fase)");
            AdicionarCampo("21", "check box", "Mudança de Curso / Transferência");
            AdicionarCampo("21", "check box", "Maiores de 23 anos / Titulares CET ou TeSP");
            AdicionarCampo("22", "texto", "Nota de ingresso");
            AdicionarCampo("22", "texto", "Data de ingresso");

            AdicionarCampo("23", "label", "3.2 Habilitação anterior");
            AdicionarCampo("24", "check box", "Ensino Secundário (12º ano)");
            AdicionarCampo("24", "check box", "Ensino Superior (Licenciatura, Mestrado, etc)");
            AdicionarCampo("25", "texto", "Instituição anterior");
            AdicionarCampo("25", "texto", "Curso anterior");
            AdicionarCampo("25", "texto", "Classificação final");

            // 4. INFORMAÇÕES (SITUAÇÃO PROFISSIONAL)
            AdicionarCampo("26", "title", "4. INFORMAÇÕES PROFISSIONAIS");
            AdicionarCampo("27", "label", "4.1 Situação do Aluno");
            AdicionarCampo("27", "check box", "Estudante");
            AdicionarCampo("27", "check box", "Trabalhador-Estudante");
            AdicionarCampo("28", "texto", "Profissão (se aplicável)");

            AdicionarCampo("29", "label", "Agregado Familiar - Situação da Mãe");
            AdicionarCampo("29", "texto", "Habilitações da Mãe");
            AdicionarCampo("29", "texto", "Profissão da Mãe");

            AdicionarCampo("30", "label", "Agregado Familiar - Situação do Pai");
            AdicionarCampo("30", "texto", "Habilitações do Pai");
            AdicionarCampo("30", "texto", "Profissão do Pai");

            // 5 a 9. DADOS FINAIS
            AdicionarCampo("31", "title", "DADOS FINAIS (Geral, Bolsa e Mobilidade)");
            AdicionarCampo("32", "check box", "Regime: Tempo inteiro");
            AdicionarCampo("32", "check box", "Regime: Tempo parcial");
            AdicionarCampo("33", "check box", "Frequência: Diurno");
            AdicionarCampo("33", "check box", "Frequência: Pós-Laboral / Noturno");

            AdicionarCampo("34", "check box switch", "Pretende candidatar-se a bolsa de estudos?");
            AdicionarCampo("35", "texto", "Se sim, qual a instituição (Ex: Ação Social, FCT, etc)");

            AdicionarCampo("36", "label", "Declarações");
            AdicionarCampo("37", "check box", "Tomei conhecimento do Regulamento Académico e de Propinas", true);
            AdicionarCampo("38", "check box", "Autorizo a reprodução do meu Cartão de Cidadão/Identificação", true);

            AdicionarCampo("39", "texto", "Data de Preenchimento", true);
            AdicionarCampo("39", "assinatura", "Assinatura do Aluno", true);

            // Criar o Formulário
            var formulario = new Formulario
            {
                Title = "Boletim de Matrícula (Mod. IPT/DSA 03.11-V1)",
                Description = "Boletim oficial de matrícula do Instituto Politécnico de Tomar com recolha de dados pessoais, fiscais e percurso académico.",
                IsActive = true,
                Fields = campos
            };

            context.Formularios.Add(formulario);
        }
    }
}
