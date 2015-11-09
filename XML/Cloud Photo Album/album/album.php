<!-- URL: http://omega.uta.edu/~pss4720/project8/album.php 
	 Name: Prawal Sharma
	 UTA ID: 1001104720
-->
<?php

// display all errors on the browser
error_reporting(E_ALL);
ini_set('display_errors','On');

// if there are many files in your Dropbox it can take some time, so disable the max. execution time
set_time_limit(0);

require_once("DropboxClient.php");

// you have to create an app at https://www.dropbox.com/developers/apps and enter details below:
$dropbox = new DropboxClient(array(
	'app_key' => "h9wy5qfiytli7b3",      // Put your Dropbox API key here
	'app_secret' => "21xqna3r57yqmmk",   // Put your Dropbox API secret here
	'app_full_access' => false,
),'en');


// first try to load existing access token
$access_token = load_token("access");
if(!empty($access_token)) {
	$dropbox->SetAccessToken($access_token);
}
elseif(!empty($_GET['auth_callback'])) // are we coming from dropbox's auth page?
{
	// then load our previosly created request token
	$request_token = load_token($_GET['oauth_token']);
	if(empty($request_token)) die('Request token not found!');
	
	// get & store access token, the request token is not needed anymore
	$access_token = $dropbox->GetAccessToken($request_token);	
	store_token($access_token, "access");
	delete_token($_GET['oauth_token']);
}

// checks if access token is required
if(!$dropbox->IsAuthorized())
{
	// redirect user to dropbox auth page
	$return_url = "http://".$_SERVER['HTTP_HOST'].$_SERVER['SCRIPT_NAME']."?auth_callback=1";
	$auth_url = $dropbox->BuildAuthorizeUrl($return_url);
	$request_token = $dropbox->GetRequestToken();
	store_token($request_token, $request_token['t']);
	die("Authentication required. <a href='$auth_url'>Click here.</a>");
}

function store_token($token, $name)
{
	if(!file_put_contents("tokens/$name.token", serialize($token)))
		die('<br />Could not store token! <b>Make sure that the directory `tokens` exists and is writable!</b>');
}

function load_token($name)
{
	if(!file_exists("tokens/$name.token")) return null;
	return @unserialize(@file_get_contents("tokens/$name.token"));
}

function delete_token($name)
{
	@unlink("tokens/$name.token");
}

function enable_implicit_flush()
{
	@apache_setenv('no-gzip', 1);
	@ini_set('zlib.output_compression', 0);
	@ini_set('implicit_flush', 1);
	for ($i = 0; $i < ob_get_level(); $i++) { ob_end_flush(); }
	ob_implicit_flush(1);
	echo "<!-- ".str_repeat(' ', 2000)." -->";
}
?>

<?php 
	if(isset($_FILES["userfile"])){
		$file = $_FILES['userfile']['name'];
		$dropbox->UploadFile($_FILES['userfile']['tmp_name'],$file);
	}
	if(isset($_GET["delete"])){
		$del_image = $_GET["delete"];
		//echo $dropbox->GetLink($del_image,false);
		$dropbox->delete($del_image);
		header("Location: album.php");
	}
?>
<html>
	<head>
		<title>Photo Album</title>
	</head>
	<body style="margin: auto; width: 1000px;">
		<div id="header" style="width:1000px; background-color: LightGrey; height: 100px;">
			<div style="padding-top: 30px; margin: auto; text-align: center;">
			<h2>Photo Album</h2>
			</div>
		</div>
		<div id="upload-form" style="width:1000px; background-color: BlanchedAlmond; height: 100px;">
		<form enctype="multipart/form-data" action="album.php" method="POST">
			<table align="center" style="padding-top: 20px;">
				<tr>
				<td>Choose file</td>
				<input type="hidden" name="MAX_FILE_SIZE" value="3000000" />
				<td><input name="userfile" type="file" /></td>
				</tr>
				<tr>
				<td></td>
				<td><input type="submit" name="submit" value="Send File" /></td>
			</table>
		</form>
		</div>
		<div id="album" style="width:1000px; background-color: AliceBlue; padding-top: 20px;">
		<?php 
				$files = $dropbox->GetFiles("",false);
				$images = array_keys($files); ?>
				<table>
				<?php 
				foreach($images as $img){?>
					<tr>
						<td><a href="?img=<?php echo $img; ?>"><?php echo $img; ?></a></td> 
						<td><a href="?delete=<?php echo $img; ?>">Delete</a></td>
					</tr>
				<?php }?>
				</table>
		</div>
		<div id="image-holder">
			<div id="image" style="height: auto; width: 400px; overflow: hidden; margin:auto;">
				<?php if(isset($_GET["img"])){
					$show_image = $_GET["img"]; ?>
					<img src="<?php echo $dropbox->GetLink($show_image,false)?>" style="height: auto; width: 300px;">
				<?php } ?>
			</div>
		</div>
	</body>
</html>
