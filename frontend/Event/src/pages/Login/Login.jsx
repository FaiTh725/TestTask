import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import ClearInput from "../../components/inputs/ClearInput/ClearInput";
import CenterScreen from "../../components/layots/CenterScreen/CenterScreen";
import EmptyLink from "../../components/links/EmptyLink/EmptyLink";
import styles from "./Login.module.css"
import logo from "../../assets/Logo.png";
import axios from "axios";
import { useEffect, useState } from "react";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const auth = useAuth();
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    email: "",
    password: ""
  });

  const [error, setError] = useState("");

  const handleChangeCredentials = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setCredentials(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleLogin = async () => {
    try
    {
      const response = await axios.get("https://localhost:5102/api/Authentication/Login?" + 
      `email=${credentials.email}&password=${credentials.password}`,
      {
        headers:{
          "Content-Type": "application/json"
        },
        withCredentials: true,
      });

      if(response.data.statusCode === 200)
      {
        localStorage.setItem("user", JSON.stringify(response.data.data));
        auth.login(response.data.data);
        navigate("/");
      }
      else
      {
        setError(response.data.description);
      }
    }
    catch
    {
      console.log("Error send form");
    }
  }

  useEffect(() => {
    if(error != "")
    {
      setError("");
    }
  }, [credentials]);

  return (
    <div className={styles.Login__Content}>
      <CenterScreen>
        <div className={styles.Login__Main}>
          <div className={styles.Login__Logo}>
            <EmptyLink link={"/"}>
              <img src={logo} alt="logo" width={50} height={50}/>
            </EmptyLink>
          </div>
          <h1 className={styles.Login__TopText}>
            Log in
          </h1>
          <h3 className={styles.Login__SubText}>
            Enter your email and password to log in
          </h3>
          <div className={styles.Login__Inputs}>
            <ClearInput 
              setValue={(e) => handleChangeCredentials(e)}
              value={credentials.email} 
              name={"email"}
              placeHolder="Email"/>
            <ClearInput 
              setValue={(e) => handleChangeCredentials(e)}
              value={credentials.password} 
              name={"password"}
              placeHolder="Password"
              typeInput="password"/>
          </div>
          <div className={styles.Login__Error}>
            <p>{error}</p>
          </div>
          <PrimaryButton text={"Continue"} action={handleLogin}/>
          <div className={styles.Login__Navigation}>
            <EmptyLink 
              link={"/account/register"} >
              <p>Sign in if dont have an account</p>
            </EmptyLink>
          </div>
        </div>
      </CenterScreen>
    </div>
  )
}

export default Login;