import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import ClearInput from "../../components/inputs/ClearInput/ClearInput";
import CenterScreen from "../../components/layots/CenterScreen/CenterScreen";
import EmptyLink from "../../components/links/EmptyLink/EmptyLink";
import styles from "./Login.module.css"

const Login = () => {
  return (
    <CenterScreen>
      <div className={styles.Login__Main}>
        <h1 className={styles.Login__TopText}>
          Log in
        </h1>
        <h3 className={styles.Login__SubText}>
          Enter your email and password to log in
        </h3>
        <div className={styles.Login__Inputs}>
          <ClearInput placeHolder="Email"/>
          <ClearInput placeHolder="Password"/>
        </div>
        <div className={styles.Login__Error}>

        </div>
        <PrimaryButton text={"Continue"}/>
        <div className={styles.Login__Navigation}>
          <EmptyLink 
            link={"/account/register"} 
            text={"Sign in if dont have an account"}/>
        </div>
      </div>
    </CenterScreen>
  )
}

export default Login;