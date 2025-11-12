window.initPriceSlider = function (dotNetRef, minValue, maxValue) {
    const slider = document.getElementById('price-slider');

    if (!slider || slider.noUiSlider) return; // already initialized

    noUiSlider.create(slider, {
        start: [minValue, maxValue],
        connect: true,
        range: {
            min: minValue,
            max: maxValue
        },
        step: 100
    });

    slider.noUiSlider.on('update', function (values) {
        document.getElementById('min-price-label').innerText = Math.round(values[0]);
        document.getElementById('max-price-label').innerText = Math.round(values[1]);
    });

    slider.noUiSlider.on('change', function (values) {
        const min = Math.round(values[0]);
        const max = Math.round(values[1]);
        dotNetRef.invokeMethodAsync('UpdatePriceRange', min, max);
    });
};
