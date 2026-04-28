-- ===========================================================================
-- MIGRACAO.SQL  --  Estrutura PostgreSQL equivalente ao ContextLocalContext
-- Gerado em: 2026-04-02
-- Banco origem: Firebird (local)
-- Banco destino: PostgreSQL
-- ===========================================================================

-- -------------------------------------------------------------------------
-- Extensão para geração de IDs sequenciais (caso não use SERIAL/BIGSERIAL)
-- -------------------------------------------------------------------------
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- ===========================================================================
-- TABELA: LOJA
-- ===========================================================================
CREATE TABLE IF NOT EXISTS loja (
    id          SERIAL          PRIMARY KEY,
    nome        VARCHAR(200)    NOT NULL DEFAULT '',
    caminho     VARCHAR(500)    NOT NULL DEFAULT '',
    host        VARCHAR(200)    NOT NULL DEFAULT '',
    porta       INTEGER         NOT NULL DEFAULT 3050,
    cnpj        VARCHAR(18)     NOT NULL DEFAULT '',
    percentual_st DOUBLE PRECISION NOT NULL DEFAULT 30
);

CREATE INDEX IF NOT EXISTS ix_loja_id ON loja (id);

-- ===========================================================================
-- TABELA: USUARIOS
-- ===========================================================================
CREATE TABLE IF NOT EXISTS usuarios (
    id              SERIAL          PRIMARY KEY,
    nome            VARCHAR(100),
    login           VARCHAR(20)     NOT NULL,
    senha           VARCHAR(1000),
    fiscal          VARCHAR(1),
    relatorio       VARCHAR(1),
    produto         VARCHAR(1),
    financeiro      VARCHAR(1),
    bloqueado       VARCHAR(1),
    dados_bloqueio  TEXT,
    empresa_id      VARCHAR(200)
);

CREATE INDEX IF NOT EXISTS ix_usuarios_login ON usuarios (login);

-- ===========================================================================
-- TABELA: PERMISSAO
-- ===========================================================================
CREATE TABLE IF NOT EXISTS permissao (
    id          SERIAL          PRIMARY KEY,
    nome        VARCHAR(1000)   NOT NULL DEFAULT '',
    descricao   VARCHAR(1000)   NOT NULL DEFAULT ''
);

-- ===========================================================================
-- TABELA: PERMISSAO_USUARIO
-- ===========================================================================
CREATE TABLE IF NOT EXISTS permissao_usuario (
    id              SERIAL  PRIMARY KEY,
    usuario_id      INTEGER NOT NULL REFERENCES usuarios (id) ON DELETE CASCADE,
    permissao_id    INTEGER NOT NULL REFERENCES permissao  (id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_permissao_usuario_usuario   ON permissao_usuario (usuario_id);
CREATE INDEX IF NOT EXISTS ix_permissao_usuario_permissao ON permissao_usuario (permissao_id);

-- ===========================================================================
-- TABELA: NCM
-- ===========================================================================
CREATE TABLE IF NOT EXISTS ncm (
    id          SERIAL          PRIMARY KEY,
    ncm         VARCHAR(20),
    descricao   VARCHAR(500),
    padrao      VARCHAR(1)
);

CREATE INDEX IF NOT EXISTS ix_ncm_id  ON ncm (id);
CREATE INDEX IF NOT EXISTS ix_ncm_ncm ON ncm (ncm);

-- ===========================================================================
-- TABELA: FORNECEDOR
-- ===========================================================================
CREATE TABLE IF NOT EXISTS fornecedor (
    cd_fornecedor   INTEGER         NOT NULL,
    id_loja         BIGINT          NOT NULL,
    nm_fornecedor   VARCHAR(200),
    PRIMARY KEY (cd_fornecedor, id_loja)
);

CREATE INDEX IF NOT EXISTS ix_fornecedor_cd   ON fornecedor (cd_fornecedor);
CREATE INDEX IF NOT EXISTS ix_fornecedor_loja ON fornecedor (id_loja);

-- ===========================================================================
-- TABELA: TIPO_VALOR_CAIXA
-- ===========================================================================
CREATE TABLE IF NOT EXISTS tipo_valor_caixa (
    id          SERIAL          PRIMARY KEY,
    descricao   VARCHAR(200)    NOT NULL
);

CREATE INDEX IF NOT EXISTS ix_tipo_valor_caixa_id ON tipo_valor_caixa (id);

-- ===========================================================================
-- TABELA: CAIXA
-- ===========================================================================
CREATE TABLE IF NOT EXISTS caixa (
    id              SERIAL          PRIMARY KEY,
    loja_id         INTEGER         NOT NULL,
    descricao       VARCHAR(100)    NOT NULL,
    data_cadastro   TIMESTAMP,
    ativo           CHAR(1)         NOT NULL DEFAULT 'V'
);

CREATE INDEX IF NOT EXISTS ix_caixa_id   ON caixa (id);
CREATE INDEX IF NOT EXISTS ix_caixa_loja ON caixa (loja_id);

-- ===========================================================================
-- TABELA: CAIXA_MOVIMENTACAO
-- ===========================================================================
CREATE TABLE IF NOT EXISTS caixa_movimentacao (
    id                      SERIAL              PRIMARY KEY,
    caixa_id                INTEGER             NOT NULL REFERENCES caixa          (id),
    tipo_valor_caixa_id     INTEGER             NOT NULL REFERENCES tipo_valor_caixa (id),
    valor                   NUMERIC(15, 2)      NOT NULL,
    data_cadastro           TIMESTAMP,
    data_competencia        TIMESTAMP,
    data_realizacao         TIMESTAMP,
    descricao               VARCHAR(100),
    nome_funcionario        VARCHAR(100),
    anexo_nome              VARCHAR(100),
    anexo_content_type      VARCHAR(10),
    anexo_arquivo           TEXT,
    ativo                   CHAR(1)             NOT NULL DEFAULT 'V'
);

CREATE INDEX IF NOT EXISTS ix_caixa_mov_id    ON caixa_movimentacao (id);
CREATE INDEX IF NOT EXISTS ix_caixa_mov_caixa ON caixa_movimentacao (caixa_id);
CREATE INDEX IF NOT EXISTS ix_caixa_mov_tipo  ON caixa_movimentacao (tipo_valor_caixa_id);
CREATE INDEX IF NOT EXISTS ix_caixa_mov_data  ON caixa_movimentacao (data_competencia);

-- ===========================================================================
-- FIM DO SCRIPT
-- ===========================================================================
