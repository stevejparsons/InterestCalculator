function RiskBounds(narrowBoundLower, narrowBoundUpper, wideBoundLower, wideBoundUpper) {
    this.narrowBoundLower = narrowBoundLower;
    this.narrowBoundUpper = narrowBoundUpper;
    this.wideBoundLower = wideBoundLower;
    this.wideBoundUpper = wideBoundUpper;
}

function RiskBoundFactory(riskLevel) {
    switch (riskLevel) {
        case "low":
            return new RiskBounds(1.5, 2.5, 1, 3);
        case "medium":
            return new RiskBounds(1.5, 3.5, 0, 5);
        case "high":
            return new RiskBounds(2, 4, -1, 7);
        default:
            throw "unknown riskLevel: " + riskLevel;
    }
}

function RequestModel(lumpSumInvestment, monthlyInvestment, timescaleInYears, narrowBoundPercentageLower, narrowBoundPercentageUpper,
    wideBoundPercentageLower, wideBoundPercentageUpper, targetValue) {
    this.lumpSumInvestment = lumpSumInvestment;
    this.monthlyInvestment = monthlyInvestment;
    this.timescaleInYears = timescaleInYears;
    this.narrowBoundPercentageLower = narrowBoundPercentageLower;
    this.narrowBoundPercentageUpper = narrowBoundPercentageUpper;
    this.wideBoundPercentageLower = wideBoundPercentageLower;
    this.wideBoundPercentageUpper = wideBoundPercentageUpper;
    this.targetValue = targetValue;
}

var viewModel = {
    isFormError: ko.observable(false),
    showGraph: ko.observable(false),
    lumpSumInvestment: ko.observable(10000).extend(
        {
            required: { params: true, message: "Please provide a lump sum investment" },
            number: { params: true, message: "Lump sum investment must be a number" },
            min: { params: 0, message: "Lump sum investment must not be a negative number" },
            max: { params: 1000000000, message: "Lump sum investment cannot be greater than 1000000000" }
        }),
    monthlyInvestment: ko.observable(250).extend(
        {
            required: { params: true, message: "Please provide a monthly investment" },
            number: { params: true, message: "Monthly investment must be a number" },
            min: { params: 0, message: "Monthly investment must not be a negative number" },
            max: { params: 1000000000, message: "Monthly investment cannot be greater than 1000000000" }
        }),
    targetValue: ko.observable(45000).extend(
        {
            required: { params: true, message: "Please provide a target investment" },
            number: { params: true, message: "Target investment must be a number" },
            min: { params: 0, message: "Target investment must not be a negative number" },
            max: { params: 1000000000, message: "Target investment cannot be greater than 1000000000" }
        }),
    timeScale: ko.observable(10).extend(
        {
            required: { params: true, message: "Please provide a timescale" },
            digit: { params: true, message: "Timescale should be a whole number" },
            min: { params: 1, message: "Timescale must be at least 1" },
            max: { params: 100, message: "Timescale cannot be greater than 100" }
        }),
    riskLevel: ko.observable("low"),

    displayGraph: function () {

        var err = viewModel.errors().length;

        if (err > 0) {
            this.isFormError(true);
            this.errors.showAllMessages();
        } else {
            this.isFormError(false);
            this.showGraph(true);
            var riskBounds = new RiskBoundFactory(this.riskLevel());

            var request = new RequestModel(this.lumpSumInvestment(), this.monthlyInvestment(), this.timeScale(), riskBounds.narrowBoundLower,
                riskBounds.narrowBoundUpper, riskBounds.wideBoundLower, riskBounds.wideBoundUpper, this.targetValue());

            getData(request);
        }
    }
}

viewModel.errors = ko.validation.group(viewModel);

ko.applyBindings(viewModel);

var chart;

function getData(request) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "http://localhost:3721/api/CalculateProjection",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (!data.Success)
                console.error(JSON.stringify(data));
            else
                displayGraph(data);
        },
        error: function (error) {
            console.error(JSON.stringify(error));
        }
    });
}

function displayGraph(data) {
    console.log(JSON.stringify(data));

    var data = {
        labels: data.Years,
        datasets: [{
            backgroundColor: '#000000',
            borderColor: '#000000',
            data: data.TargetValue,
            fill: false,
            label: 'Target value'
        }, {
            backgroundColor: '#7fff7f', 
            borderColor: '#00ff00',
            data: data.NarrowBoundUpperSeries,
            label: 'Narrow bound upper',
            fill: false,
        }, {
            backgroundColor: '#7fff7f', 
            borderColor: '#00ff00',
            data: data.NarrowBoundLowerSeries,
            label: 'Narrow bound lower',
            fill: '-1'
        }, {
            backgroundColor: '#ff7f7f', 
            borderColor: '#ff0000',
            data: data.WideBoundUpperSeries,
            fill: false,
            label: 'Wide bound upper'
        }, {
            backgroundColor: '#ff7f7f', 
            borderColor: '#ff0000',
            data: data.WideBoundLowerSeries,
            label: 'Wide bound lower',
            fill: '-1'
        }]
    };

    var options = {
        responsive: true,
        tooltips: {

            callbacks: {
                title: function () {
                    return '';
                },
                beforeLabel: function (tooltipItem, data) {
                    return 'Year ' + ': ' + tooltipItem.xLabel;
                },
                label: function (tooltipItem, data) {
                    return data.datasets[tooltipItem.datasetIndex].label + ': ' + tooltipItem.yLabel;
                },
                afterLabel: function (tooltipItem, data) {
                    return '';
                }
            }
        },
        maintainAspectRatio: false,
        spanGaps: false,
        elements: {
            line: {
                tension: 0.000001
            }
        },
        scales: {
            xAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Years'
                }
            }],
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Value (£)'
                }
            }]
        },
        plugins: {
            filler: {
                propagate: false
            },
            samples_filler_analyser: {
                target: 'chart-analyser'
            }
        }
    };

    if (chart !== undefined) {
        chart.destroy();
    }

    chart = new Chart('chartProjection', {
        type: 'line',
        data: data,
        options: options
    });
}
