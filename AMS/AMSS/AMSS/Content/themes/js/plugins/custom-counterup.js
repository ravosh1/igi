 $(document).ready(function(){
  var executed = false;
     $('.counter-up').waypoint(function() {
      if (!executed) {
        executed = true;

           $('#yoe').animateNumber
           (
              {
               number: 40.00,
               easing: 'ease', 
              },
              2000
           );
           
           $('#hc').animateNumber
           (
              {
               number: 200.00,
               easing: 'ease', 
              },
              2000
           );
           $('#satis').animateNumber
           (
              {
               number: 4000.00,
               easing: 'ease', 
              },
              12000
           );
		   $('#month4').animateNumber
           (
              {
               number: 13.50,
               easing: 'ease', 
              },
              12000
           );

           $('#employees').animateNumber
           (
              {
               number: 25,
               easing: 'ease', 
              },
              2000
           );
           $('#income').animateNumber
           (
              {
               number: 95,
               easing: 'ease', 
              },
              2000
           );
           $('#lowerCosts').animateNumber
           (
              {
               number: 78,
               easing: 'ease', 
              },
              2000
           );

        }
        

    },{offset: 'bottom-in-view' })

// =====================================================
 });