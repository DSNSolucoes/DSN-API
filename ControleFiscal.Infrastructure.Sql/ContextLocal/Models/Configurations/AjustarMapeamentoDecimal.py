import os

# Obtém o diretório onde o script está sendo executado
directory_path = os.path.dirname(os.path.abspath(__file__))

# Percorre todos os arquivos na pasta e subpastas
for root, dirs, files in os.walk(directory_path):
    for file in files:
        # Verifica se o arquivo termina com "Configuration.cs"
        if file.endswith("Configuration.cs"):
            file_path = os.path.join(root, file)
            
            # Lê o conteúdo do arquivo
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Substitui as ocorrências de 'decimal' por 'decimal(18,3)'
            new_content = content.replace("DECIMAL", "DECIMAL(18,3)")
            
            # Escreve as alterações de volta no arquivo apenas se houver mudanças
            if new_content != content:
                with open(file_path, 'w', encoding='utf-8') as f:
                    f.write(new_content)
                print(f"Arquivo modificado: {file_path}")

print("Alteração de 'decimal' para 'decimal(18,3)' em todos os arquivos *Configuration.cs concluída.")
