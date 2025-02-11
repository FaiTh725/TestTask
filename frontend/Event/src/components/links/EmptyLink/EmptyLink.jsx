import { useNavigate } from "react-router-dom";
import styles from "./EmptyLink.module.css";

const EmptyLink = ({text, link}) => {
  const navigate = useNavigate();

  return (
    <span className={styles.EmptyLink__Main} 
      onClick={() => navigate(link)}>
      {text}
    </span>
  )
}

export default EmptyLink;