<!-- Website:omega.uta.edu/~pss4720/project3/buy.php
	Name: Prawal Sharma
	UTA ID: 1001104720 -->

	<?php 
	session_start();
?>

<html>
<head>
<title>Buy Products</title>
<style>
	body{
		width: 1000px;
		margin: auto;
	}
</style>
</head>
<body>

<?php  
	if(isset($_GET["clear"])){
		$_SESSION["items"] = array();
		$_SESSION["tp"] = 0 ;
	}
	if(isset($_GET["id"])){
			try{
			//Getting Product by product id.
			$product_details = 'http://sandbox.api.ebaycommercenetwork.com/publisher/3.0/rest/GeneralSearch?apiKey=78b0db8a-0ee1-4939-a2f9-d3cd95ec0fcc&visitorUserAgent&visitorIPAddress&trackingId=7000610&productId='.$_GET["id"];
			$product_contents = file_get_contents($product_details);
			$xml_product = new SimpleXMLElement($product_contents);
			if(!array_key_exists($_GET["id"],$_SESSION["items"])){
					$_SESSION["tp"] += (double)$xml_product->categories->category->items->product->minPrice;
			}
			$_SESSION["items"][$_GET["id"]]["id"] = (string)$xml_product->categories->category->items->product['id'];
			$_SESSION["items"][$_GET["id"]]["img"] = (string)$xml_product->categories->category->items->product->images->image->sourceURL;
			$_SESSION["items"][$_GET["id"]]["price"] = (double)$xml_product->categories->category->items->product->minPrice;
			$_SESSION["items"][$_GET["id"]]["name"] = (string)$xml_product->categories->category->items->product->name;
			$_SESSION["items"][$_GET["id"]]["url"] = (string)$xml_product->categories->category->items->product->productOffersURL;
			}catch(Exception $e){
				
			}
	}
	if(isset($_GET["delete"])){
		if(array_key_exists($_GET["delete"],$_SESSION["items"])){
			$_SESSION["tp"] = (double)$_SESSION["tp"] - (double)$_SESSION["items"][$_GET["delete"]]["price"];
			unset($_SESSION["items"][$_GET["delete"]]);
		}
	}
?>
<div id="basket">
	<label>Shopping Basket:</label>
	<p><?php if(isset($_SESSION["items"])){ ?>
	<table border='1'>
		<tbody>
			<?php try{ ?>
			<?php foreach($_SESSION["items"] as $item_product){ ?>
				<tr>
					<td><a href='<?php echo $item_product["url"] ?>'><img src='<?php echo $item_product['img'] ?>' /></a></td>
					<td><?php echo $item_product['name'] ?></td>
					<td>$<?php echo $item_product['price'] ?></td>
					<td><a href='?delete=<?php echo $item_product['id'] ?>'>Delete</a></td>
				</tr>
			<?php } ?>
			<?php }catch(Exception $e){
			
			} ?>
		</tbody>
	</table>
	<?php try{ ?> 
	<label>Total: $<?php echo $_SESSION["tp"]; ?></label>
	<?php }catch(Exception $e){
	
	} ?>
	<form action="buy.php" method="GET">
		<input type="hidden" value="1" name="clear" />
		<input type="Submit" value="Empty Cart"/>
	</form>
	<?php } ?>
	</p>
</div>
<?php 
	$xml_file = file_get_contents('http://sandbox.api.ebaycommercenetwork.com/publisher/3.0/rest/CategoryTree?apiKey=78b0db8a-0ee1-4939-a2f9-d3cd95ec0fcc&visitorUserAgent&visitorIPAddress&trackingId=7000610&categoryId=72&showAllDescendants=true');
	$list_xml = new SimpleXMLElement($xml_file);
?>
<div id="search">
	<form method="GET" action="buy.php">
		<label>Select Categories:
			<select name="catid">
				<option value='<?php echo $list_xml->category['id'] ?>'><?php echo $list_xml->category->name ?></option>
				<?php 
					foreach($list_xml->category->categories->category as $cat){ ?>
						<optgroup label='<?php echo $cat->name ?>' value='<?php echo $cat['id'] ?>'>
							<option value='<?php echo $cat['id'] ?>'><?php echo $cat->name ?></option>
							<?php 
								foreach($cat->categories->category as $subcat){ ?>
									<option value='<?php echo $subcat['id'] ?>'><?php echo $subcat->name ?></option>
							<?php 
							}
							?>
						</optgroup>
					<?php } ?>
			</select>
		</label>
		<label>Search Keywords: 
			<input type="text" name="search" />
		</label>
		<label>
			<input type="submit" name="Submit"/>
		</label>
	</form>
</div>
<?php 
	if(isset($_GET["Submit"])){
		$search_keyword = $_GET["search"];
		$search_cat = $_GET["catid"];
		if($search_keyword == " "){
			$query_string = 'http://sandbox.api.ebaycommercenetwork.com/publisher/3.0/rest/GeneralSearch?apiKey=78b0db8a-0ee1-4939-a2f9-d3cd95ec0fcc&visitorUserAgent&visitorIPAddress&trackingId=7000610&categoryId='.$search_cat.'&numItems=20';
		}else{
			$query_string = 'http://sandbox.api.ebaycommercenetwork.com/publisher/3.0/rest/GeneralSearch?apiKey=78b0db8a-0ee1-4939-a2f9-d3cd95ec0fcc&trackingId=7000610&category='.$search_cat.'&keyword='.$search_keyword.'&numItems=20';
		}
		$to_send = str_replace(" ","%20",$query_string); //Problems with amazon servers accepting space in search query
		$search_response = file_get_contents($to_send);
		$xml_search_response = new SimpleXMLElement($search_response);
		?>
		<table border='1'>
			<tbody>
				<?php foreach($xml_search_response->categories->category->items->product as $pro) {?>
					<tr>
						<td><a href='?id=<?php echo $pro['id'] ?>'><img src='<?php echo $pro->images->image->sourceURL ?>' /></a></td>
						<td><?php echo $pro->name; ?></td>
						<td>$<?php echo $pro->minPrice; ?></td>
						<td><?php echo $pro->fullDescription; ?></td>
					</tr>
				<?php } ?>
			</tbody>
		</table>
	<?php } ?>
<?php

error_reporting(E_ALL);
ini_set('display_errors','Off');
/*$xmlstr = file_get_contents('http://sandbox.api.ebaycommercenetwork.com/publisher/3.0/rest/GeneralSearch?apiKey=78b0db8a-0ee1-4939-a2f9-d3cd95ec0fcc&trackingId=7000610&keyword=sony+vaio');
$xml = new SimpleXMLElement($xmlstr);
header('Content-Type: text/xml');
print $xmlstr;
*/
?>
</body>
</html>
