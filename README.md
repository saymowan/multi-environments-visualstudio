# multi-environments-visualstudio

Projeto com arquivo de configuração (app.config) com variáveis de configuração. Para este tutorial foi utilizado o software Microsoft Visual Studio Enterprise 2017.
Exemplo de arquivo e chaves:

![enter image description here](https://i.imgur.com/W0ZvGfH.png)

## Criar configurações para diferentes ambientes:

Criação dos ambientes através do ConfigurationManager no menu Compilação/Gerenciador de Configurações:

 ![enter image description here](https://i.imgur.com/GbhVyk5.png)

## Criar Arquivo de Configuração do Aplicativo
Para cada ambiente, incluir para o projeto um novo Arquivo de Configuração do Aplicativo com o padrão: `App.NOME-AMBIENTE.config`
Para o nosso exemplo serão criados:
 - app.prod.config
 - app.desenv.config

Após a crição os mesmos deverão ser listados juntamente com o arquivo de configuração padrão do projeto. O próximo passo é gerar dependência destes novos arquivos de ambientes com o arquivo padrão.


![enter image description here](https://i.imgur.com/JPnol25.png)

## Atualização csproj com novas regras
Existem duas configurações para serem incluídas no arquivo csproj do nosso projeto:

 - Dependência dos novos arquivos de configurações com o arquivo default
 - Incluir trigger para executar ações após o build

Editar o arquivo csproj do projeto em alguma ferramenta (Sublime Text, Visual Code, Notepad):


![enter image description here](https://i.imgur.com/iLl5KyQ.png)


Encontrar o ItemGroup referente aos arquivos de configuração:


![enter image description here](https://i.imgur.com/5s16nnK.png)


Faça as alterações abaixo onde a tag DependentUpon está associada somente para os novos arquivos de configurações que substituirão o arquivo atual:

    <ItemGroup>
    <None Include="app.config">
    <SubType>Designer</SubType>
    </None>
    <None Include="app.prod.config">
	    <DependentUpon>app.config</DependentUpon>
    <SubType>Designer</SubType>
    </None>
    <None Include="app.desenv.config">
	    <DependentUpon>app.config</DependentUpon>
    <SubType>Designer</SubType>
    </None>
    <None Include="license" />
    <None Include="packages.config" />
    </ItemGroup>

Ao salvar o csproj e recarregar o projeto, será possível verificar que existe uma dependência de um arquivo raiz (app.config) e os demais ficarão em um subnível:


![enter image description here](https://i.imgur.com/2p51tIC.png)

Agora é necessário incluir a trigger para executar ações após o build dentro do arquivo csproj:

    <Target Name="AfterBuild">
    <Delete Files="$(TargetDir)$(TargetFileName).config" />
    <Copy SourceFiles="$(ProjectDir)\app.$(Configuration).config" DestinationFiles="$(TargetDir)$(TargetFileName).config" />
    </Target>

Poderá ser incluído em qualquer parte do arquivo desde que esteja dentro da tag `<Project> </Project>`

![enter image description here](https://i.imgur.com/ZBYzTsK.png)

## Validando alteração de ambiente (environment)

Foi configurado um pacote nuget NUnit para listagem de métodos de testes e um método de teste que exibe no console as chaves comuns presentes dentro do arquivo de configuração: `nome_branch ` e `database`:

    public class UnitTest1
    {
        [Test]
        public void ValidarAmbientes()
        {
            Console.WriteLine("Nome Branch: " + ConfigurationManager.AppSettings["nome_branch"].ToString());
            Console.WriteLine("Database: " + ConfigurationManager.AppSettings["database"].ToString());
        }
    }

Ao escolher o ambiente "desenv":

![enter image description here](https://i.imgur.com/HWbV19s.png)

As configurações serão utilizadas conforme chaves e valores dentro do arquivo app.desenv.config:
![enter image description here](https://i.imgur.com/JteOh16.png)

Basta compilar e executar o método ValidarAmbientes() para verificar o uso da nova configuração na saída do console (output):


![enter image description here](https://i.imgur.com/cGFIce4.png)

Caso eu troque para a outra configuração disponível "prod" e realize o mesmo processo ficará da seguinte forma:


![enter image description here](https://i.imgur.com/CYJ06o2.png)


Para transformações parciais ou de branches, será necessário o uso de scripts Ansible, Powershell ou demais recursos que fazem este tipo de alteração.

