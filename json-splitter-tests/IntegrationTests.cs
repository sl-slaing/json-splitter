using json_splitter;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace json_splitter_tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [TestCaseSource(nameof(Scenarios))]
        public void TestScenario(Scenario scenario)
        {
            var configRepository = new Mock<IConfigurationRepository>();
            var serialiser = new JsonSerializer();
            var senderFactory = new Mock<IDataSenderFactory>();
            var streamFactory = new Mock<IStreamFactory>();
            var mockDataSender = new FileDataSender(serialiser, new Mock<FileConfiguration>().Object, streamFactory.Object);
            var inputFactory = new Mock<IInputFactory>();
            var outputStreams = new Dictionary<string, StringWriter>();

            configRepository
                .Setup(r => r.ReadConfiguration(It.IsAny<string>()))
                .Returns(scenario.Configuration);
            senderFactory
                .Setup(f => f.GetDataSender(It.IsAny<IDataConfiguration>()))
                .Returns(mockDataSender);
            inputFactory
                .Setup(f => f.GetInput(It.IsAny<Arguments>()))
                .Returns(new StringReader(string.Join(Environment.NewLine, scenario.NdJson)));
            streamFactory
                .Setup(f => f.OpenWrite(It.IsAny<string>()))
                .Returns<string>((fileName) =>
                {
                    if (!outputStreams.ContainsKey(fileName))
                    {
                        outputStreams.Add(fileName, new StringWriter());
                    }

                    return outputStreams[fileName];
                });

            var processor = new Processor(
                new NoOpProgressReporter(),
                serialiser,
                configRepository.Object,
                new DataProcessor(
                    new DataSenderFactory(serialiser, streamFactory.Object),
                    new RelationalObjectReader()),
                inputFactory.Object);

            processor.Execute(new Arguments());

            Assert.That(outputStreams.Keys, Is.EquivalentTo(scenario.ExpectedFiles.Keys));
            foreach (var actualOutput in outputStreams)
            {
                Assert.That(
                    actualOutput.Value.GetStringBuilder().ToString(),
                    Is.EqualTo(string.Join(Environment.NewLine, scenario.ExpectedFiles[actualOutput.Key]) + Environment.NewLine));
            }
        }

        public static IEnumerable<Scenario> Scenarios
        {
            get
            {
                yield return ParentAndChild_OneJsonObject;
            }
        }

        private static Scenario ParentAndChild_OneJsonObject = new Scenario
        {
            Configuration = new RelatedJsonConfiguration
            {
                Relationships = new Dictionary<string, RelatedJsonConfiguration>
                {
                    { "children", new RelatedJsonConfiguration
                        {
                            File = new FileConfiguration
                            {
                                ForeignKeyColumnName = "parent_id",
                                ForeignKeyPropertyName = "id",
                                FileName = "child.csv"
                            }
                        } }
                },
                File = new FileConfiguration
                {
                    FileName = "parent.csv"
                }
            },
            NdJson = new[]
            {
                "{'id': 1, 'name': 'Dad', 'children': [ { 'id': '1.1', 'name': 'Son' }, { 'id': '1.2', 'name': 'Daughter' } ] }"
            },
            ExpectedFiles =
            {
                {"parent.csv", new[]
                    {
                        "id,name",
                        "1,Dad"
                    } },
                { "child.csv", new[]
                    {
                        "id,name,parent_id",
                        "1.1,Son,1",
                        "1.2,Daughter,1"
                    } }
            }
        };
    }
}
