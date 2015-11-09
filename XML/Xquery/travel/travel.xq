1. for $a in doc("travel.xml")/travel/flight
	let $x := doc("travel.xml")/travel/airport
	where $a/date = "11/20/2014"
		and $a/@from =  $x[name = "DFW"]/@code
		and $a/@to = $x[name = "JFK"]/@code	
	return data($a)
	
2. for $x in doc("travel.xml")/travel/passenger 
return 
	<passenger> { data($x),
		let $a := doc("travel.xml")/travel/reservation[@passenger = $x/@ssn]
		return 
			<reservation> {count($a)} </reservation>
	} </passenger>
	
3. for $x in doc("travel.xml")/travel/airport
	return 
		<airport>{
			data($x), <code>{data($x/@code)}</code>,
			<departure>{
				let $a:=doc("travel.xml")/travel/flight[@from = $x/@code]
				return count($a)
			}</departure>, 
			<arrival>{
				let $b:=doc("travel.xml")/travel/flight[@to = $x/@code]
				return count($b)
			}</arrival>
		}</airport>
		
4. 	for $a in doc("travel.xml")/travel/flight
	let $x := doc("travel.xml")/travel/airport
	where $a/@from =  $x[name = "DFW"]/@code	
	return <details>{
			<date>{data($a/date)}</date>, 
			<departure>{data($a/@from), data($a/departureTime)}</departure>,  
			<arrival>{data($a/@to), data($a/arrivalTime)}</arrival>,
			let $r:=doc("travel.xml")/travel/reservation
			where $r/@flight = $a/@id
			return <reservations>{count($r)}</reservations>
			}
			</details>
			
5. for $x in doc("travel.xml")/travel/passenger
	where $x/name = "John Smith"
return
<name>
{	data($x/name),
	let $a:=doc("travel.xml")/travel/reservation[@passenger=$x/@ssn]
	return <reservation>{count($a)}</reservation>
}</name>

6. for $n in doc("travel.xml")/travel/passenger
     where $n/name ="John Smith"
      return ( for $r in doc("travel.xml")/travel/reservation
                where $n/@ssn = $r/@passenger
                 return ( for $f in doc("travel.xml")/travel/flight
                           where $f/@id = $r/@flight
                          return (for $a in doc("travel.xml")/travel/airport
                                   where $a/@code = $f/@to
                                   return {$a/name} )))  